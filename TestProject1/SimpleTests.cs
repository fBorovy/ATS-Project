using IDE.Parser;
using IDE.PQLParser;

using Testing;

namespace TestProject1;

[TestClass]
public sealed class SimpleTests
{
    private static QueryParser queryParser;

    [ClassInitialize]
    public static void ClassSetup(TestContext ctx)
    {
        CodeParser parser = new CodeParser($"{AppDomain.CurrentDomain.BaseDirectory}\\..\\..\\..\\..\\IDE\\SIMPLE.txt");
        parser.ReadFile();
        parser.Parse();
        queryParser = new QueryParser();
    }

    [DataTestMethod]
    [DataRow("assign a;Select a such that Follows (a, 17)", "none")]
    [DataRow("assign a;Select a such that Uses (a, \"top\")", "9,67,131,135")]
    [DataRow("if ifstat;Select ifstat such that Follows* (ifstat, 17)", "14,15")]
    [DataRow("procedure p;Select p such that Calls (p, \"Shift\")", "Main")]
    [DataRow("procedure p;Select p such that Calls (p, \"XX\")", "SS,UU")]
    [DataRow("procedure p;Select p such that Calls* (p, \"Scale\")", "none")]
    [DataRow("procedure p;Select p such that Calls* (p, \"XX\")", "SS,UU,TT,PP,RR")]
    [DataRow("procedure p;Select p such that Uses (p, \"left\")", "Main,Init,Random,Transform,Shift")]
    [DataRow("Select a Pattern a (_,\"incre * weight\")", "134")]
    [DataRow("Select a Pattern a (_,\"y2 - y1\")", "141,168")]
    [DataRow("stmt s;Select a such that Modifies (s, \"right\")", "125,131")]
    [DataRow("stmt s;Select s such that Follows (s, 20)", "19")]
    [DataRow("stmt s;Select s such that Follows (s, 294)", "none")]
    [DataRow("stmt s;Select s such that Follows* (s, 248)", "244,245,246,247")]
    [DataRow("stmt s;Select s such that Follows* (s, 39)", "none")]
    [DataRow("if ifstat;Select ifstat such that Follows* (ifstat, 17)", "none")]
    [DataRow("stmt s;Select s such that Modifies (s, \"left\")", "124,130")]
    [DataRow("stmt s;Select s such that Modifies (s, \"x1\")", "7,67,120,138,164,165,174,182,245,252")]
    [DataRow("stmt s;Select s such that Parent (143, s)", "144,145,146,147,148")]
    [DataRow("stmt s;Select s such that Parent (s, 137)", "none")]
    [DataRow("stmt s;Select s such that Parent (s, 139)", "138")]
    [DataRow("stmt s;Select s such that Parent* (s, 141)", "140,136")]
    [DataRow("stmt s;Select s such that Uses (s, \"left\")", "7,30,49,107,124,130,135,152,153,154,155,156,157,158")]
    [DataRow("stmt s;Select s such that Uses (s, \"mtoggle\")", "192")]
    [DataRow("stmt s;Select s such that Uses (s, \"right\")", "8,31,109,125,131,135,152,153,154,155,156,157,158")]
    [DataRow("variable v;Select v such that Modifies (v, \"UU\")", "cs6,cs5,cs9,cs8")]
    [DataRow("variable v;Select v such that Modifies (v, \"XX\")", "cs6,cs5")]
    [DataRow("variable v;Select v such that Uses (301,v)", "cs9,cs5,cs6,cs8,")]
    [DataRow("variable v;Select v such that Uses (309,v)", "cs5,cs6")]
    [DataRow("while w;Select w such that Modifies (w, \"tmp\")", "6,12,16,29,47,59,79,90,95,105,136,180,181,187")]
    [DataRow("while w;Select w such that Parent* (w, 27)", "26,16,12,6")]
    public void KamilTests(string query, string expectedResult)
    {
        //var result = queryParser.ParseWithExceptions(query);
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult.Normalize(), result.Normalize());
    }


    [DataTestMethod]
    [DataRow("stmt s;Select s such that Parent(s, 25)", "23")]
    [DataRow("procedure p;Select p such that Calls(\"Init\", p) ", "none")]
    [DataRow("variable v;Select v such that Uses (20, v)", "difference")]
    [DataRow("stmt s,s1;Select s such that Parent(s, s1) with s1.stmt#= 12", "6")]
    [DataRow("stmt s;Select s such that Follows(s, 9)", "8")]
    [DataRow("if ifs; Select ifs such that Parent*(ifs, 185)", "184,181,180")]
    [DataRow("procedure p;Select p such that Modifies (p, \"x1\")", "Main,Init,Transform,Shear,Move,Shrink")]
    [DataRow("variable v;Select v such that Uses (10, v)", "y1,incre,bottom")]
    [DataRow("procedure p;Select p such that Modifies (p, \"top\")", "Main,Init,Shift")]
    [DataRow("if ifs;Select ifs such that Parent (ifs, 16)", "15")]
    [DataRow("stmt s;Select s such that Uses(s, \"y\")", "none")]
    [DataRow("while w; Select w such that Parent(w, 10)", "6")]
    [DataRow("if ifstmt;Select ifstmt such that Follows*(ifstmt, 7)", "none")]
    [DataRow("variable v;Select v such that Modifies (4, v)", "tmp")]
    [DataRow("while w;Select w such that Parent* (w, 13)", "12, 6")]
    [DataRow("procedure p; Select BOOLEAN such that Calls(p, \"Scale\")", "FALSE")]
    [DataRow("stmt s,s1;Select s such that Parent(s, s1) with s1.stmt#= 28", "26")]
    [DataRow("assign a;Select a such that Modifies(a, \"x4\")", "153")]
    [DataRow("assign a;Select a such that Follows(a, 8)", "7")]
    [DataRow("stmt s;Select s such that Follows* (s, 19)", "17,18")]
    [DataRow("procedure p;Select p such that Calls(p, \"Draw\")", "Main,Shrink")]
    [DataRow("stmt s; Select s such that Follows*(1, s)", "2,3,4,5,6,119")]
    [DataRow("stmt s; Select s such that Follows*(119, s)", "none")]
    [DataRow("stmt s;Select s such that Parent* (s, 30)", "29,16,15,14,12,6")]
    [DataRow("stmt s; Select s such that Parent(s, 221)", "218")]
    [DataRow("procedure p; Select p such that Modifies(p, \"x3\")", "Main,Shift")]
    [DataRow("assign a;Select a such that Modifies(a, \"reduce\")", "none")]
    [DataRow("procedure p;Select p such that Calls(p, \"Draw\")", "Main,Shrink")]
    [DataRow("stmt s; Select s such that Parent* (s, 10)", "6")]
    [DataRow("stmt s, s1; Select s such that Follows*(s, s1) with s1.stmt# = 6", "1,2,3,4,5")]

    public void EwaTests(string query, string expectedResult)
    {
        //var result = queryParser.ParseWithExceptions(query);
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult.Normalize(), result.Normalize());
    }


    [DataTestMethod]
    [DataRow("assign a; while w;Select a such that Follows (w, a)", "11, 46, 68, 82, 102, 135, 183, 186, 255, 263, 380")]
    [DataRow("stmt s, s1;Select s such that Parent (s, s1) with s1.stmt# = 77", "76")]
    [DataRow("call c;Select c such that Follows (c, 85)", "84")]
    [DataRow("call c; stmt s;Select c such that Follows* (c, s) with s1.stmt# = 6", "5")]
    [DataRow("if ifs; variable v;Select ifs such that Uses (ifs, \"volume\")", "55")]
    [DataRow("if ifs; while w;Select w such that Parent (if, w)", "16, 26, 79, 83, 89, 113, 217, 239, 251, 256, 279, 281")]
    [DataRow("if ifs; while w;Select ifs such that Parent (if, w) with ifs.stmt# = 15", "15")]
    [DataRow("procedure p, q;Select q such that Calls (p, q) with p.procName = \"Draw\"", "Clear, Random, Show")]
    [DataRow("stmt s;Select s such that Uses (s, \"height\")", "9, 11, 32, 40, 46, 86, 137, 148, 186, 263, 273")]
    [DataRow("stmt s;Select s such that Parent*(34, s)", "35,36,37,38,39,40,41,42,43,44")]
    [DataRow("variable v; procedure p;Select v such that Uses (p, v) with p.procName = \"Init\"", "120,121,122,123,124,125,126,127,128,129")]
    [DataRow("while w;Select w with w.stmt# = 143", "143")]
    [DataRow("assign a; if ifs;Select a such that Parent (ifs, a) with ifs.stmt# = 176", "177,178")]
    [DataRow("procedure p;Select p such that Uses (p, \"asterick\")", "Draw")]
    [DataRow("stmt s;Select s such that Uses (s, asterick)", "196,206")]
    [DataRow("assign a;Select a such that Modifies (a, \"blue\")", "215")]
    [DataRow("procedure p; Select p such that Calls (p, \"Show\")", "Draw,Enlarge,Rotate")]
    [DataRow("stmt s;Select s such that Uses (s, \"wrong\")", "278")]
    [DataRow("stmt s;Select s such that Follows* (s, 287)", "283,284,285,268")]
    [DataRow("stmt s;Sekect s such that Modifies (s, \"cs5\")", "290,298,302,311")]
    [DataRow("procedure p;Select p", "Main,Init,Random,Transform,Shift,Shear,Move,Draw,Clear,Show,Enlarge,Fill,Shrink,Translate,Rotate,Scale,PP,QQ,SS,TT,UU,XX")]
    [DataRow("while w; variable v;Select w such that Uses (w, \"top\")", "59,89")]
    [DataRow("assign a;Select a such that Modifies (a, \"top\")", "77,126,150")]
    [DataRow("assign a;Select a such that Uses (a, \"top\")", "9,67,131,135")]
    [DataRow("while w; stmt s;Select w such that Parent (w, s) with s.stmt# = 186", "181")]
    [DataRow("while w; stmt s;Select s such that Parent (w, s) with w.stmt# = 181", "182,183,184,186,187")]
    [DataRow("stmt s;Select s such that Parent (s, 100)", "97")]
    [DataRow("Select BOOLEAN such that Uses (8, \"jaja\")", "FALSE")]
    [DataRow("procedure p, q;Select BOOLEAN such that Calls(p,q)", "TRUE")]
    [DataRow("Select BOOLEAN such that Modifies(999, \"x\")", "FALSE")]
    public void FilipTests(string query, string expectedResult)
    {
        //var result = queryParser.ParseWithExceptions(query);
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult.Normalize(), result.Normalize());
    }


    [DataTestMethod]
    [DataRow("procedure p; Select p", "Main Init Random Transform Shift Shear Move Draw Clear Show Enlarge Fill Shrink Translate Rotate Scale PP QQ RR SS TT UU XX")]
    [DataRow("stmt s; Select s", "1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67 68 69 70 71 72 73 74 75 76 77 78 79 80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95 96 97 98 99 100 101 102 103 104 105 106 107 108 109 110 111 112 113 114 115 116 117 118 119 120 121 122 123 124 125 126 127 128 129 130 131 132 133 134 135 136 137 138 139 140 141 142 143 144 145 146 147 148 149 150")]
    [DataRow("variable v; Select v", "x1 x2 y1 y2 left right top bottom incre decrement radius volume width height area difference tmp length distance x3 x4 x5 x6 x7 x8 x9 b c pink green blue pixel dot line edge semi depth notmove total triangle base dx dy factor cs1 cs2 cs3 cs4 cs5 cs6 cs8 cs9 mtoggle lengx cover marking median asterick range peak trim unknown correct wrong wcounter s p1 p2")]
    [DataRow("procedure p; Select p such that Calls(p, \"Init\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Draw\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Clear\")", "Draw")]
    [DataRow("procedure p; Select p such that Calls(p, \"Show\")", "Draw Enlarge Rotate")]
    [DataRow("procedure p; Select p such that Calls(p, \"QQ\")", "PP TT")]
    [DataRow("procedure p; Select p such that Calls(p, \"UU\")", "TT")]
    [DataRow("procedure p; Select p such that Calls(p, \"XX\")", "SS TT UU")]
    [DataRow("procedure p; Select p such that Calls(p, \"Translate\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Rotate\")", "Translate")]
    [DataRow("procedure p; Select p such that Calls(p, \"Shear\")", "Main")]
    [DataRow("procedure p; Select p such that Modifies(p, \"x1\")", "Main Init Random Transform Shift Shear Move Draw Clear Show Enlarge Fill Shrink Translate Rotate")]
    [DataRow("procedure p; Select p such that Modifies(p, \"width\")", "Main Transform")]
    [DataRow("procedure p; Select p such that Modifies(p, \"height\")", "Main Transform")]
    [DataRow("procedure p; Select p such that Modifies(p, \"tmp\")", "Main Transform Shift Shear Move Draw Enlarge Shrink Translate Rotate")]
    [DataRow("procedure p; Select p such that Modifies(p, \"volume\")", "Main")]
    [DataRow("procedure p; Select p such that Uses(p, \"top\")", "Main Init Random Transform Shift Enlarge Shrink")]
    [DataRow("procedure p; Select p such that Uses(p, \"bottom\")", "Main Init Random Transform Shift Enlarge Shrink")]
    [DataRow("procedure p; Select p such that Uses(p, \"left\")", "Main Init Random Transform Shift Shrink")]
    [DataRow("procedure p; Select p such that Uses(p, \"right\")", "Main Init Random Transform Shift Shrink")]
    [DataRow("procedure p; Select p such that Uses(p, \"k\")", "Main")]
    [DataRow("procedure p; Select p such that Uses(p, \"incre\")", "Main Init Random Transform Shift Shear Move Draw Enlarge Shrink Translate Rotate")]
    [DataRow("procedure p; Select p such that Modifies(p, \"radius\")", "Main")]
    [DataRow("procedure p; Select p such that Uses(p, \"radius\")", "Main")]
    [DataRow("procedure p; Select p such that Uses(p, \"x4\")", "Main")]
    [DataRow("procedure p; Select p such that Modifies(p, \"c\")", "Main")]
    [DataRow("procedure p; Select p such that Uses(p, \"tmp\")", "Main Transform Shift Shear Move Draw Enlarge Shrink Translate Rotate")]
    [DataRow("procedure p; Select p such that Modifies(p, \"depth\")", "Enlarge Fill")]
    [DataRow("procedure p; Select p such that Uses(p, \"depth\")", "Enlarge Fill Rotate")]

    public void JanTests(string query, string expectedResult)
    {
        //var result = queryParser.ParseWithExceptions(query);
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult.Normalize(), result.Normalize());
    }


    [DataTestMethod]
    [DataRow("stmt s; Select s such that Modifies(s, \"radius\")", "28,30,33")]
    [DataRow("procedure p; Select p such that Uses(p, \"tmp\")", "Main,Transform,Shrink")]
    [DataRow("assign a; Select a such that Affects(33, a)", "34")]
    [DataRow("assign a; Select a such that Affects*(28, a)", "33,34")]
    [DataRow("stmt s; Select s such that Parent(27, s)", "28,29,30,31")]
    [DataRow("stmt s; Select s such that Parent*(15, s)", "16,17,18,19,20,21,22")]
    [DataRow("stmt s; Select s such that Uses(s, \"x1\")", "13,28,33")]
    [DataRow("stmt s; Select s such that Modifies(s, \"x4\")", "33")]
    [DataRow("procedure p; Select p such that Calls*(p, \"Draw\")", "Main,Shrink")]
    [DataRow("procedure p; Select BOOLEAN such that Calls*(p, \"Enlarge\")", "TRUE")]
    [DataRow("stmt s; Select s such that Follows(27, s)", "28")]
    [DataRow("stmt s; Select s such that Follows*(27, s)", "28,29,30,31")]
    [DataRow("stmt s; Select s such that Parent(s, 33)", "32")]
    [DataRow("if ifs; Select ifs such that Parent(ifs, 35)", "34")]
    [DataRow("assign a; Select a such that Modifies(a, \"length\")", "36,40")]
    [DataRow("stmt s; Select s such that Modifies(s, \"area\")", "18,25")]
    [DataRow("stmt s; Select s such that Uses(s, \"area\")", "19,30,34")]
    [DataRow("assign a; Select a such that Uses(a, \"bottom\")", "16")]
    [DataRow("while w; Select w such that Parent(w, 36)", "35")]
    [DataRow("stmt s; Select s such that Uses(s, \"incre\")", "13,14,15,16,18,28,33")]
    [DataRow("stmt s; Select s such that Uses(s, \"right\")", "14")]
    [DataRow("stmt s; Select s such that Follows(s, 25)", "26")]
    [DataRow("stmt s; Select s such that Parent(34, s)", "35,36,37,38")]
    [DataRow("stmt s; Select s such that Affects(30, s)", "33")]
    [DataRow("stmt s; Select s such that Affects(s, 34)", "33")]
    [DataRow("stmt s; Select s such that Affects*(30, 34)", "TRUE")]
    [DataRow("assign a;Select a such that Uses(a, \"x\")", "12, 22, 45, 78")]
    [DataRow("stmt s;Select s such that Modifies(s, \"result\")", "33, 44")]
    [DataRow("while w;Select w such that Follows*(5, w)", "6, 7")]
    [DataRow("call c;Select c such that Calls(\"Init\", c)", "none")]
    [DataRow("stmt s;Select s such that Parent*(s, 100)", "95, 96, 97, 98")]
    public void KasiaTests(string query, string expectedResult)
    {
        //var result = queryParser.ParseWithExceptions(query);
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult.Normalize(), result.Normalize());
    }


    [DataTestMethod]
    [DataRow("stmt s; Select s such that Follows(1, s)", "2")]
    [DataRow("stmt s; Select s such that Follows(2, s)", "3")]
    [DataRow("stmt s; Select s such that Follows(3, s)", "4")]
    [DataRow("stmt s; Select s such that Follows(s, 3)", "2")]
    [DataRow("stmt s; Select s such that Follows(s, 2)", "1")]
    [DataRow("stmt s; Select s such that Parent(6, s)", "7")]
    [DataRow("stmt s; Select s such that Parent(12, s)", "13")]
    [DataRow("stmt s; Select s such that Parent(s, 14)", "13")]
    [DataRow("stmt s; Select s such that Parent*(12, s)", "16")]
    [DataRow("stmt s; Select s such that Parent(6, s)", "7")]
    [DataRow("stmt s; Select s such that Modifies(2, \"width\")", "2")]
    [DataRow("stmt s; Select s such that Modifies(4, \"tmp\")", "4")]
    [DataRow("stmt s; Select s such that Modifies(s, \"height\")", "3")]
    [DataRow("procedure p; Select p such that Modifies(p, \"width\")", "Main")]
    [DataRow("procedure p; Select p such that Modifies(p, \"decrement\")", "Init")]
    [DataRow("stmt s; Select s such that Uses(9, \"top\")", "9")]
    [DataRow("stmt s; Select s such that Uses(8, \"x1\")", "8")]
    [DataRow("stmt s; Select s such that Uses(31, \"right\")", "31")]
    [DataRow("stmt s; Select s such that Uses(s, \"height\")", "11")]
    [DataRow("procedure p; Select p such that Uses(p, \"height\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(\"Main\", p)", "Init Random")]
    [DataRow("procedure p; Select p such that Calls(p, \"Init\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Random\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Draw\")", "Main")]
    [DataRow("procedure p; Select p such that Calls(p, \"Translate\")", "Main")]
    [DataRow("stmt s; Select s such that Affects(30, s)", "32")]
    [DataRow("stmt s; Select s such that Affects(s, 50)", "46")]
    [DataRow("stmt s; Select s such that Affects(46, 50)", "50")]
    [DataRow("stmt s; Select s such that Affects(1, s)", "none")]
    [DataRow("stmt s; Select s such that Affects(32, s)", "none")]
    public void MaciekTests(string query, string expectedResult)
    {
        //var result = queryParser.ParseWithExceptions(query);
        var result = queryParser.ParseQuery(query);
        Assert.AreEqual(expectedResult.Normalize(), result.Normalize());
    }
}
