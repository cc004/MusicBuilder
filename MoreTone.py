import os

ins = [
   "acpiano",   "britepno",  "synpiano",  "honkytonk", "epiano1",   "epiano2",
   "hrpschrd",  "clavinet",  "celeste",   "glocken",   "musicbox",  "vibes",
   "marimba",   "xylophon",  "tubebell",  "santur",    "homeorg",   "percorg",
   "rockorg",   "churchorg", "reedorg",   "accordn",   "harmonica", "concrtna",
   "nyguitar",  "acguitar",  "jazzgtr",   "cleangtr",  "mutegtr",   "odguitar",
   "distgtr",   "gtrharm",   "acbass",    "fngrbass",  "pickbass",  "fretless",
   "slapbas1",  "slapbas2",  "synbass1",  "synbass2",  "violin",    "viola",
   "cello",     "contraba",  "marcato1",   "pizzcato",  "harp",      "timpani",
   "marcato2",  "slowstr",   "synstr1",   "synstr2",   "choir",     "doo",
   "voices",    "orchhit",   "trumpet",   "trombone",  "tuba",      "mutetrum",
   "frenchorn", "hitbrass",  "synbras1",  "synbras2",  "sprnosax",  "altosax",
   "tenorsax",  "barisax",   "oboe",      "englhorn",  "bassoon",   "clarinet",
   "piccolo",   "flute",     "recorder",  "woodflut",  "bottle",    "shakazul",
   "whistle",   "ocarina",   "sqrwave",   "sawwave",   "calliope",  "chiflead",
   "charang",   "voxlead",   "lead5th",   "basslead",  "fantasia",  "warmpad",
   "polysyn",   "ghostie",   "bowglass",  "metalpad",  "halopad",   "sweeper1",
   "aurora",    "soundtrk",  "crystal",   "atmosphr",  "freshair",  "unicorn",
   "sweeper2",   "startrak",  "sitar",     "banjo",     "shamisen",  "koto",
   "kalimba",   "bagpipes",  "fiddle",    "shannai",   "carillon",  "agogo",
   "steeldrum", "woodblock", "taiko",     "toms",      "syntom",    "revcymb",
   "fxfret",    "fxblow",    "seashore",  "jungle",    "telephone", "helicptr",
   "applause",  "ringwhsl"]

item = open('Items\\Midi.cs', 'w')
tile = open('Tiles\\Midi.cs', 'w')

item.write('using Project_Logic.Registry;\nusing Project_Logic.Items;\nusing Terraria;\nusing System;\n\nnamespace MusicBuilder.Items\n{\n')
tile.write('using Project_Logic.Registry;\n\nnamespace MusicBuilder.Tiles\n{\n')

for i in range(0, 128):
    item.write('    public class Midi_{} : Noteblock\n'.format(ins[i]))
    item.write('    {\n')
    item.write('        public override void AddRecipes() {}\n')
    item.write('        public override Prog NOTE\n')
    item.write('        {\n')
    item.write('            get\n')
    item.write('            {\n')
    item.write('                return (Prog) {};\n'.format(1024 + i))
    item.write('            }\n')
    item.write('        }\n')
    item.write('    }\n')
    item.write('\n')
    tile.write('    public class Midi_{} : NoteblockEx\n'.format(ins[i]))
    tile.write('    {\n')
    tile.write('        public override Prog NOTE\n')
    tile.write('        {\n')
    tile.write('            get\n')
    tile.write('            {\n')
    tile.write('                return (Prog) {};\n'.format(1024 + i))
    tile.write('            }\n')
    tile.write('        }\n')
    tile.write('    }\n')
    tile.write('\n')

item.write('}\n')
tile.write('}\n')
