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
    unsigned int time;
    unsigned short lasting;
    unsigned char instrument, pitch, velocity;
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
        notes[i].time =  getUInt(mem, 12 * i + 4);
        notes[i].lasting = getUShort(mem, 12 * i + 8);
        notes[i].instrument = getByte(mem, 12 * i + 10);
        notes[i].pitch = getByte(mem, 12 * i + 11);
        notes[i].velocity = getByte(mem, 12 * i + 12);
        cout << (int)notes[i].instrument << ' ' << (int)notes[i].pitch << ' ' << notes[i].time << ' ' << (int)notes[i].lasting << endl;
    }
}

int main()
{
    readMidi("D:\\touhou.mid", callback, nullptr);
    cout << sizeof(struct Note);
}
