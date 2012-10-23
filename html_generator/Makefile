%.exe : %.cs
	mcs -out:"$@" "$<"

%.out : %.exe
	mono "$<" | tee "$@"
