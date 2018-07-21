using System;


namespace NetApp.Droid.Jivosdk
{
    public interface IJivoDelegate
    {
        void onEvent(String name, String data);
    }
}