using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace web_frames
{
	interface IGenerator
	{
		Object generate();
	}

	class string_generator : IGenerator
	{
		string m_content;
		public string_generator(string content = "")
		{
			m_content = content;
		}
		public virtual Object generate() { return m_content; }
	}

	class file_generator : IGenerator
	{
		string m_file_name;
		public file_generator(string file_name)
		{
			m_file_name = file_name;
		}
		public virtual Object generate()
		{
			return System.IO.File.ReadAllText(m_file_name);
		}
	}

	class Generators : List<IGenerator>, IGenerator
	{
		public virtual Object generate()
		{
			var buf = new StringBuilder();
			foreach (var i in this)
			{
				buf.Append(i.generate());
			}
			return buf;
		}
	}

	class Page : IGenerator
	{
		public IGenerator m_header;
		public IGenerator m_footer;
		public IGenerator m_body;
		public IGenerator m_style_sheet;
		public Object generate()
		{
			return "" + 
				m_header.generate() 
				+ "<style>\n" + m_style_sheet.generate() + "</style>\n"
				+ m_body.generate() + m_footer.generate();
		}
	}

	class program
	{
		static void Main(string[] args)
		{
			//string path = "~/csci310/week_2/"; //not neccesary in linux because linux understands paths like the non mentally retarded child that it is
			//##########
			//# Page 1 #
			//##########
			Page page1 = new Page();
			page1.m_header = new string_generator ("<!doctype: html>\n");
			
			var body1 = new Generators();
			body1.Add(new string_generator ("<body>\n"));
			body1.Add(new file_generator ("banner.html"));
			body1.Add(new file_generator ("menu.html"));
			body1.Add(new string_generator ("<p>Hi Dr. Macavoy you should explain things at a lower level, your last class was overwhelming.</p>\n"));	
			body1.Add(new string_generator ("</body>\n"));

			page1.m_body = body1;
			page1.m_footer = new string_generator("</html>");
			page1.m_style_sheet = new file_generator ("pg1.css");

			Console.WriteLine("page1:\n" + page1.generate());

			//##########
			//# Page 2 #
			//##########
			
			Page page2 = new Page();
			page2.m_header = new string_generator ("!doctype: html>\n");

			var body2 = new Generators();
			body2.Add (new string_generator ("<body\n"));
			body2.Add (new file_generator ("banner.html"));
			body2.Add (new file_generator ("menu.html"));
			body2.Add (new file_generator ("awesomesauce"));
			body2.Add (new string_generator ("</body>\n"));
			
			page2.m_body = body2;
			page2.m_footer = new string_generator("</html>");
			page2.m_style_sheet = new file_generator ("pg2.css");

			Console.WriteLine("\npage2:\n" + page2.generate());
		}
	}
}
