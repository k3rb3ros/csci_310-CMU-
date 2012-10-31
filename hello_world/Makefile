%.exe : %.cs
	mcs -out:"$@" "$<"

%.out : %.exe
	mono "$<" | tee "$@"

args.out : args.exe
	mono args.exe "Run" "With" "Args" | tee args.out
