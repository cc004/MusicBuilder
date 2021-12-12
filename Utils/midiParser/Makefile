obj = src/Binasc.o src/MidiEvent.o src/MidiEventList.o src/MidiFile.o src/MidiMessage.o src/Options.o
CXXFLAG = -O3 -std=c++11 -m32 -Os
LDFLAG = -shared -static-libstdc++ -static-libgcc -m32 -Os

all: midiParser.dll #$(obj) test.exe
src/%.o: src/%.cpp
	g++ $< -o $@ -c -I.\include $(CXXFLAG)
tools/%.o: tools/%.cpp
	g++ $< -o $@ -c -I.\include $(CXXFLAG)
%.dll: tools/%.o $(obj)
	g++ $< $(obj) -o $@ $(LDFLAG)
%.exe: tools/%.o tools/midiParser.o $(obj)
	g++ $< tools/midiParser.o $(obj) -o $@ -static -m32