using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
// using Storj2;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Asn1.Sec;
using System.Data.Entity.Validation;
using System.Configuration;
using System.Web;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Threading;
using System.Reflection;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Org.BouncyCastle.Crypto.Encodings;
using Telerik.Web.UI;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using System.Globalization;
using System.IO.Compression;
using System.Net.Sockets;

public class StorjBase : System.Web.UI.Page
{
    public bool IsPortOpen(string IP, int Port)
    {
        var result = false;
        using (var client = new TcpClient())
        {
            try
            {
                client.ReceiveTimeout = 1 * 1000;
                client.SendTimeout = 1 * 1000;
                var asyncResult = client.BeginConnect(IP, Port, null, null);
                var waitHandle = asyncResult.AsyncWaitHandle;
                try
                {
                    if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1), false))
                    {
                        // wait handle didn't came back in time
                        client.Close();
                    }
                    else
                    {
                        // The result was positiv
                        result = client.Connected;
                    }
                    // ensure the ending-call
                    client.EndConnect(asyncResult);
                }
                finally
                {
                    // Ensure to close the wait handle.
                    waitHandle.Close();
                }
            }
            catch
            {
            }
        }
        return result;

    }

    public static string UURAND()
    {
        // Returns a random number.
        int randomvalue = 0;
        using (RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider())
        {
            byte[] rno = new byte[32];
            rg.GetBytes(rno);
            randomvalue = BitConverter.ToInt32(rno, 0);
        }
        // return randomvalue.ToString();
        return Guid.NewGuid().ToString();


    }

}


