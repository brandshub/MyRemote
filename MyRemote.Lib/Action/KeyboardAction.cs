using MyRemote.Lib.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;


namespace MyRemote.Lib.Action
{
    public class KeyboardAction : CommandAction
    {

        public const string CODE = "KEYBOARD";
        public const string INPUT = "input";

        public override string Code => CODE;
        public KeyboardAction(Dictionary<string, string> parameters) : base(parameters)
        {

        }


        public override CommandResponse Execute()
        {
            string input = this[INPUT].ToUpper();

            // Keys.Add
            var x = new InputSimulator();

            if (input == "ALT+TAB")
            {
                x.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MENU);
                System.Threading.Thread.Sleep(50);
                x.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
                System.Threading.Thread.Sleep(50);
                x.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.MENU);
            }
            else
            {
                ParseInput(input, out VirtualKeyCode[] mods, out VirtualKeyCode[] keys);
                if (mods == null || mods.Length == 0)
                {
                    x.Keyboard.KeyPress(keys);
                }
                else
                {
                    x.Keyboard.ModifiedKeyStroke(mods, keys);
                }
            }
            return new CommandResponse();
        }

        //private static Regex r_parseInput = new Regex(@"(?<mod>.+?\+)*?(?<tail>.)");

        public static void ParseInput(string input, out VirtualKeyCode[] modifiers, out VirtualKeyCode[] buttons)
        {
            var inputUc = input.ToUpper().Replace("CTRL", "LCONTROLKEY").Replace("SHIFT", "LSHIFTKEY").Replace("ALT", "LMENU").Replace("WIN", "LWIN");
            var parts = inputUc.Split('+');

            var mods = new List<VirtualKeyCode>();
            var bts = new List<VirtualKeyCode>();

            for (int i = 0; i < parts.Length; i++)
            {
                if (int.TryParse(parts[i], out int res))
                    parts[i] = "D" + parts[i];

                if (parts[i] == "LCONTROLKEY" || parts[i] == "LSHIFTKEY" || parts[i] == "LMENU" || parts[i] == "LWIN")
                {
                    if (vkMap.TryGetValue(parts[i], out int key))
                        mods.Add((VirtualKeyCode)key);
                    else
                        Debug.WriteLine("Not found VK for " + parts[i]);
                }
                else
                {
                    if (vkMap.TryGetValue(parts[i], out int key))
                        bts.Add((VirtualKeyCode)key);
                    else
                        Debug.WriteLine("Not found VK for " + parts[i]);
                }

            }

            modifiers = mods.ToArray();
            buttons = bts.ToArray();
        }

        public override CommandRequest RequestThis()
        {
            return new CommandRequest
            {
                Id = this.Id + "_AUTO",
                ActionId = Id,
                Parameters = new Dictionary<string, string>(Parameters)
            };
        }

        private static Dictionary<string, int> vkMap = new Dictionary<string, int>
        {
{"NONE",0},{"LBUTTON",1},{"RBUTTON",2},{"CANCEL",3},{"MBUTTON",4},{"XBUTTON1",5},{"XBUTTON2",6},{"BACK",8},{"TAB",9},{"LINEFEED",10},{"CLEAR",12},{"ENTER",13},{"RETURN",13},{"SHIFTKEY",16},{"CONTROLKEY",17},{"MENU",18},{"PAUSE",19},{"CAPSLOCK",20},{"CAPITAL",20},{"HANGULMODE",21},{"HANGUELMODE",21},{"KANAMODE",21},{"JUNJAMODE",23},{"FINALMODE",24},{"KANJIMODE",25},{"HANJAMODE",25},{"ESCAPE",27},{"IMECONVERT",28},{"IMENONCONVERT",29},{"IMEACCEPT",30},{"IMEACEEPT",30},{"IMEMODECHANGE",31},{"SPACE",32},{"PRIOR",33},{"PAGEUP",33},{"PAGEDOWN",34},{"NEXT",34},{"END",35},{"HOME",36},{"LEFT",37},{"UP",38},{"RIGHT",39},{"DOWN",40},{"SELECT",41},{"PRINT",42},{"EXECUTE",43},{"SNAPSHOT",44},{"PRINTSCREEN",44},{"INSERT",45},{"DELETE",46},{"HELP",47},{"D0",48},{"D1",49},{"D2",50},{"D3",51},{"D4",52},{"D5",53},{"D6",54},{"D7",55},{"D8",56},{"D9",57},{"A",65},{"B",66},{"C",67},{"D",68},{"E",69},{"F",70},{"G",71},{"H",72},{"I",73},{"J",74},{"K",75},{"L",76},{"M",77},{"N",78},{"O",79},{"P",80},{"Q",81},{"R",82},{"S",83},{"T",84},{"U",85},{"V",86},{"W",87},{"X",88},{"Y",89},{"Z",90},{"LWIN",91},{"RWIN",92},{"APPS",93},{"SLEEP",95},{"NUMPAD0",96},{"NUMPAD1",97},{"NUMPAD2",98},{"NUMPAD3",99},{"NUMPAD4",100},{"NUMPAD5",101},{"NUMPAD6",102},{"NUMPAD7",103},{"NUMPAD8",104},{"NUMPAD9",105},{"MULTIPLY",106},{"ADD",107},{"SEPARATOR",108},{"SUBTRACT",109},{"DECIMAL",110},{"DIVIDE",111},{"F1",112},{"F2",113},{"F3",114},{"F4",115},{"F5",116},{"F6",117},{"F7",118},{"F8",119},{"F9",120},{"F10",121},{"F11",122},{"F12",123},{"F13",124},{"F14",125},{"F15",126},{"F16",127},{"F17",128},{"F18",129},{"F19",130},{"F20",131},{"F21",132},{"F22",133},{"F23",134},{"F24",135},{"NUMLOCK",144},{"SCROLL",145},{"LSHIFTKEY",160},{"RSHIFTKEY",161},{"LCONTROLKEY",162},{"RCONTROLKEY",163},{"LMENU",164},{"RMENU",165},{"BROWSERBACK",166},{"BROWSERFORWARD",167},{"BROWSERREFRESH",168},{"BROWSERSTOP",169},{"BROWSERSEARCH",170},{"BROWSERFAVORITES",171},{"BROWSERHOME",172},{"VOLUMEMUTE",173},{"VOLUMEDOWN",174},{"VOLUMEUP",175},{"MEDIANEXTTRACK",176},{"MEDIAPREVIOUSTRACK",177},{"MEDIASTOP",178},{"MEDIAPLAYPAUSE",179},{"LAUNCHMAIL",180},{"SELECTMEDIA",181},{"LAUNCHAPPLICATION1",182},{"LAUNCHAPPLICATION2",183},{"OEMSEMICOLON",186},{"OEM1",186},{"OEMPLUS",187},{"OEMCOMMA",188},{"OEMMINUS",189},{"OEMPERIOD",190},{"OEM2",191},{"OEMQUESTION",191},{"OEM3",192},{"OEMTILDE",192},{"OEM4",219},{"OEMOPENBRACKETS",219},{"OEMPIPE",220},{"OEM5",220},{"OEMCLOSEBRACKETS",221},{"OEM6",221},{"OEMQUOTES",222},{"OEM7",222},{"OEM8",223},{"OEM102",226},{"OEMBACKSLASH",226},{"PROCESSKEY",229},{"PACKET",231},{"ATTN",246},{"CRSEL",247},{"EXSEL",248},{"ERASEEOF",249},{"PLAY",250},{"ZOOM",251},{"NONAME",252},{"PA1",253},{"OEMCLEAR",254},{"KEYCODE",65535}
        };

        static KeyboardAction()
        {
          /*  var keyNames = Enum.GetNames(typeof(System.Windows.Forms.Keys));
            var keyValues = Enum.GetValues(typeof(System.Windows.Forms.Keys)).Cast<int>().ToArray();

            vkMap = new Dictionary<string, VirtualKeyCode>(keyNames.Length - 4);
            for (int i = 0; i < keyNames.Length - 4; i++)
            {
                vkMap.Add(keyNames[i].ToUpper(), (VirtualKeyCode)(keyValues[i]));
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(vkMap);*/
        }


    }
}
