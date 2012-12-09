using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui_threaded_messenger_thing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string host = "";
        int port = 0;
        static bool send = false;
        static bool cl_is_con = false;
        static bool client_is_alive = false;
        static bool server_is_alive = false;
        static bool srv_is_con = false;
        static string alias = "Ryan:";

        void connect(string host, int port)
        {
            client_is_alive = true;
            new Thread(Client).Start();//start the client
        }

        public MainWindow()
        {
            InitializeComponent();
            Label2.Text = alias;
            output.IsReadOnly = true;
            host = host_addr.Text;
            port = 6669; 
            server_is_alive = true;
            new Thread(Server).Start();//start the server
        }

        void Client()
        {
            NetworkStream netStream = null;
            TcpClient client = new TcpClient();
            while (client_is_alive)
            {
                if (!cl_is_con)
                {
                    try
                    {
                        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(host), port);//figure out how to pass these from main
                        client.Connect(serverEndPoint); //atempt to connect
                    }
                    catch
                    {
                        MessageBox.Show("Error connecting");
                        client_is_alive = false;
                    }
                    finally
                    {
                        output.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            output.Text = "Connected to " + host + ":" + port;
                        }
                        ));
                        cl_is_con = true;
                    }
                }
                else
                {
                    if (send == true) //send 
                    {
                        byte[] message = null;
                        input.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            message = Encoding.UTF8.GetBytes(input.Text); //experimental code
                        }
                        ));
                       
                        netStream = client.GetStream(); //spawn a netstream
                        netStream.Write(message, 0, message.Length); //send the message
                        input.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            input.Text = ""; //Clear the input buffer
                        }
                        ));
                        //input_buffer = ""; //Clear the input box
                        send = false; //Set the send flag to false
                    }
                }
            }
            client.Close();
        }

        void Server()
        {
            TcpClient remote = null;
            TcpListener server = new TcpListener(IPAddress.Any, port); //Create a listener and wait for someone to connect
            server.Start();
            while (server_is_alive)
            {
                if (!srv_is_con)
                {
                    try
                    {
                        remote = server.AcceptTcpClient(); //wait for connection
                    }
                    catch //something bad happened shutdown both threads
                    {
                        MessageBox.Show("Server error terminating");
                        server_is_alive = false;
                        client_is_alive = false;
                    }
                    finally
                    {
                        srv_is_con = true;
                    }
                }
                else
                {
                    //Get data
                    NetworkStream clientStream = remote.GetStream();
                    byte[] message = new byte[4096];
                    int bytesRead;
                    while (true)
                    {
                        bytesRead = 0;
                        try
                        {
                            //blocks until a client sends a message
                            bytesRead = clientStream.Read(message, 0, 4096);
                        }
                        catch
                        {
                            MessageBox.Show("error reading from socket.");
                            //a socket error has occured
                            break;
                        }
                        if (bytesRead == 0)
                        {
                            //the client has disconnected from the server
                            break;
                        }
                        //message has successfully been received
                        UTF8Encoding encoder = new UTF8Encoding();
                        //ASCIIEncoding encoder = new ASCIIEncoding();
                        string smsg = encoder.GetString(message, 0, bytesRead);
                        if (smsg == "quit")
                        {
                            client_is_alive = false;
                            server_is_alive = false;
                        }
                        output.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
                        {
                            output.Text = smsg; //Clear the input buffer
                        }
                        ));
                    }
                    remote.Close();
                }
            }
        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            if (!client_is_alive)
            {
                connect(host, port);
                output.Text = "Connecting...";
            }
            else MessageBox.Show("Connection in progress");
        }
        private void snd_btn_Click(object sender, RoutedEventArgs e)
        {
            output.Text = "Sending...";
            send = true; //Set the send flag to true and the client thread will take care of rest
        }
    }
}
