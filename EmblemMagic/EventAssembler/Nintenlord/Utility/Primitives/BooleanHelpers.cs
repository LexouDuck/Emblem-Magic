namespace Nintenlord.Utility.Primitives
{
    public static class BooleanHelpers
    {
        public static bool Eq(this bool a, bool b)
        {
            return a == b;
        }

        public static bool Imp(this bool a, bool b)
        {
            return !a || b;
        }
    }
}

/*Test
         
    StringBuilder builder = new StringBuilder("Result:\n");
    builder.AppendLine("Eq:");
    TestBool(BooleanHelpers.Eq, builder);
    builder.AppendLine("Imp:");
    TestBool(BooleanHelpers.Imp, builder);
    builder.AppendLine("And:");
    TestBool((x, y) => x && y, builder);
    builder.AppendLine("Or:");
    TestBool((x, y) => x || y, builder);
    builder.AppendLine("Xor:");
    TestBool((x, y) => x ^ y, builder);
    MessageBox.Show(builder.ToString());
         

static void TestBool(Func<bool, bool, bool> func, StringBuilder buffer)
{
    buffer.AppendLine("False False => " + func(false, false));
    buffer.AppendLine("True  False => " + func(true, false));
    buffer.AppendLine("False True  => " + func(false, true));
    buffer.AppendLine("True  True  => " + func(true, true));
}
         
 */