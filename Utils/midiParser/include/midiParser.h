#ifndef _MIDIPARSER
#define _MIDIPARSER

#ifdef __cplusplus
extern "C"
{
#endif
    void readMidi(const char *, void (*)(void *, void *), void *);
    unsigned int getUInt(char *, int);
    unsigned char getByte(char *, int);
    unsigned short getUShort(char *, int);
#ifdef __cplusplus
}
#endif

#endif