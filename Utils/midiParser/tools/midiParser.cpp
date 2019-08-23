#include "MidiFile.h"
#include "midiParser.h"

#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;
using namespace smf;

struct Note
{
    unsigned int time;
    unsigned short lasting;
    unsigned char instrument, pitch, velocity;
    Note(int instrument, int pitch, int time, int velocity):instrument(instrument), pitch(pitch), velocity(velocity), time(time) {}
    Note() : velocity(0) {}
};

void readMidi(const char *filename, void callback(void *, void *), void *args)
{
    MidiFile midifile(filename);
    vector<Note> notes;
    int channel_instrument[16];
    int volume[16];
    Note *record[16];

    double tempo = 1.0, abstick; //midi tick per game tick
    unsigned int lasttime, tpp = midifile.getTicksPerQuarterNote();

    for (int i = 0; i < 16; ++i)
    {
        channel_instrument[i] = 0;
        volume[i] = 127;
        record[i] = new Note[128];
    }

    midifile.absoluteTicks();
    midifile.joinTracks();

    abstick = 0.0;
    lasttime = 0;

    for (int i = 0; i < midifile.getNumEvents(0); ++i)
    {
        int command = midifile[0][i][0] & 0xf0;
        int channel = midifile[0][i][0] & 0x0f;

        abstick += (midifile[0][i].tick - lasttime) / tempo;
        if (midifile[0][i][0] == 0xff && midifile[0][i][1] == 0x51)
        {
            int microseconds = 0;
            microseconds = microseconds | (midifile[0][i][3] << 16);
            microseconds = microseconds | (midifile[0][i][4] << 8);
            microseconds = microseconds | (midifile[0][i][5] << 0);
            tempo = tpp * 1000000.0 / (60 * microseconds);
        }

        if (command == 0x90 && midifile[0][i][2] != 0)
        {
            if (record[channel][midifile[0][i][1]].velocity && midifile[0][i][2]) 
            {
                record[channel][midifile[0][i][1]].lasting = min(abstick - record[channel][midifile[0][i][1]].time, 65535.0);
                if (record[channel][midifile[0][i][1]].lasting)
                    notes.push_back(record[channel][midifile[0][i][1]]);
                record[channel][midifile[0][i][1]].velocity = 0;
            }

            if (channel == 0x09)
                record[channel][midifile[0][i][1]] = Note(
                    128, //instrument = drum
                    midifile[0][i][1], //pitch
                    abstick, //time
                    midifile[0][i][2] * volume[channel] / 127 //velocity
                );
            else
                record[channel][midifile[0][i][1]] = Note(
                    channel_instrument[channel], //instrument
                    midifile[0][i][1], //pitch
                    abstick, //time
                    midifile[0][i][2] * volume[channel] / 127 //velocity
                );
        }
        else if (command == 0x90 || command == 0x80)
        {
            if (record[channel][midifile[0][i][1]].velocity) 
            {
                record[channel][midifile[0][i][1]].lasting =  min(abstick - record[channel][midifile[0][i][1]].time, 65535.0);
                if (record[channel][midifile[0][i][1]].lasting)
                    notes.push_back(record[channel][midifile[0][i][1]]);
                record[channel][midifile[0][i][1]].velocity = 0;
            }
        }
        else if (command == 0xc0)
            channel_instrument[channel] = midifile[0][i][1];
        else if (command == 0xb0)
        {
            switch (midifile[0][i][1])
            {
                case 0x07:
                    volume[channel] = midifile[0][i][2];
                    break;
            }
        }
        lasttime = midifile[0][i].tick;
    }

    sort(notes.begin(), notes.end(), [](const Note& x, const Note& y) -> bool {return x.time < y.time;});
    //0 noteSize
    //4 ~ 12*n+4 data
    char *mem;
    Note *p;
    mem = new char[notes.size() * sizeof(Note) + 4];
    *(unsigned int *)mem = notes.size();
    p = (Note *)(mem + sizeof(unsigned int));
    for (const Note& note : notes)
        *p++ = note;
    callback(mem, args);
    delete[] mem;
    for (int i = 0; i < 16; ++i)
        delete[] record[i];
}

unsigned int getUInt(char *mem, int index)
{
    return *(unsigned int *)&mem[index];
}

unsigned char getByte(char *mem, int index)
{
    return *(char *)&mem[index];
}

unsigned short getUShort(char *mem, int index)
{
    return *(unsigned short *)&mem[index];
}
