using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Encryption{

    // Use this for initialization
    static int GetNumber(char ch)
    {
        switch(ch)
        {
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            case '0':return ch - '0';
            case 'a':
            case 'b':
            case 'c':
            case 'd':
            case 'e':return ch - 'a' + 10;
            default: return 15;
        }
    }
    static char GetChar(int num)
    {
        switch (num)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 0: return (char)(num + '0');
            case 10:return 'a';
            case 11:return 'b';
            case 12:return 'c';
            case 13:return 'd';
            case 14:return 'e';
            default: return 'f';
        }
    }
	public static bool IsRIghtDevices () {
        char[] op =  SystemInfo.deviceUniqueIdentifier.ToCharArray();
        int len = op.Length;
        for (int i = 0; i < len; i++)
        {
            int now = GetNumber(op[i]);
            int end = GetNumber(op[len - i - 1]);
            now = ((now * 17) % (end + 1)) + 16;
            now %= 16;
            op[i] = GetChar(now);
        }
        string ans = new string(op);
        try
        {
            StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/text/password/password.flykey", System.Text.Encoding.Default);
            string obj = sr.ReadLine();
            sr.Close();
            if (obj != ans)
            {
                Application.Quit();
                return false;
            }
            else return true;
        }
        catch(Exception e)
        {
            Application.Quit();
        }
        return false;
	}
	
}
