using System;

public static class FontManager
{
    public static string DotNumberFont(string str) {
        string rtnStr = "";
        if (str == null)
            return rtnStr;
        for (int i = 0; i < str.Length; i++) {
            string convStr;
            switch (str[i]) {
               case '-':
                    convStr = "10";
                    break;
                default:
                    convStr = str[i].ToString();
                    break;
            }
            rtnStr += "<sprite=" + convStr + ">";
        }
        return rtnStr;
    }
}
