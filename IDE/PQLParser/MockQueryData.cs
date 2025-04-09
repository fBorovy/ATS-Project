using System;

namespace IDE.PQLParser;

public class MockQueryData
{
    public static List<Tuple<String, String>> queries = [
        Tuple.Create("stmt s1, s2;","Select s1 such that Parent (s1, s2) with s2.stmt# = 5"),
        Tuple.Create("stmt s1, s2;","Select s2 such that Parent (s1, s2)"),
        Tuple.Create("stmt s1, s2;","Select s2 such that Parent (s1, s2) with s1.stmt# = 5"),
        Tuple.Create("stmt s1, s2;","Select s2 such that Parent (s1, s2)"),
        Tuple.Create("stmt s1, s2;","Select s1 such that Follows (s1, s2) with s2.stmt# = 5"),
        Tuple.Create("stmt s1, s2;","Select s1 such that Follows (s1, s2)"),
        Tuple.Create("stmt s1, s2;","Select s2 such that Follows (s1, s2)"),
        Tuple.Create("stmt s1, s2;","Select s2 such that Follows (s1, s2) with s1.stmt# = 7"),
        Tuple.Create("assign a; variable v;","Select v such that Modifies (a, v)"),
        Tuple.Create("assign a; variable v;","Select v such that Modifies (a, v) with a.stmt# = 21"),
        Tuple.Create("assign a; variable v;","Select a such that Modifies (a, v) with v.varName=\"x\""),
        Tuple.Create("assign a; variable v;","Select a such that Uses (a, v)"),
        Tuple.Create("assign a; variable v;","Select a such that Uses (a, v) with v.varName=\"x\""),
        Tuple.Create("stmt s1, s2;","Select s1 such that Follows* (s1, s2)"),
    ];
}
