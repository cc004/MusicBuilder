#include <iostream>
#include "midiParser.h"
using namespace std;

extern "C"
{
    void readMidi(const char *, void (*)(void *, void *), void *);
    unsigned int getUInt(char *, int);
    unsigned char getByte(char *, int); 
}

struct Note
{
    unsigned char instrument, pitch, velocity, lasting;
    unsigned int time;
};

void callback(void *mem0, void *val)
{
    Note *notes;
    char *mem = (char *)mem0;
    cout << "callback called" << endl;
    unsigned size = getUInt(mem, 0);
    notes = new Note[size];
    for (int i = 0; i < size; ++i)
    {
        notes[i].instrument = getByte(mem, 8 * i + 4);
        notes[i].pitch = getByte(mem, 8 * i + 5);
        notes[i].velocity = getByte(mem, 8 * i + 6);
        notes[i].lasting = getByte(mem, 8 * i + 7);
        notes[i].time =  getUInt(mem, 8 * i + 8);
        cout << (int)notes[i].instrument << ' ' << (int)notes[i].pitch << ' ' << notes[i].time << ' ' << (int)notes[i].lasting << endl;
    }
}

int main()
{
    readMidi("D:\\lemon.mid", callback, nullptr);
}
