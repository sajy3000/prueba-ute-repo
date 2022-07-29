using System;
using System.Security.Cryptography;

// https://docs.microsoft.com/en-us/dotnet/standard/security/cryptography-model
public class StoreKey
{
    public static void Main()
    {
        try
        {
            // // Generate o read Pub/Pri Key to or from the container
            var parameters = new CspParameters
            {
                KeyContainerName = "MyKeyContainer"
            };

            
            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            var rsa = new RSACryptoServiceProvider(parameters);
            
            // // WritePubKey to xml file SI FALSE - GUARDA SOLO LA PUBLICA - TRUE GRABA LA PRIVADA Y PUBLICA
            var publicKey = rsa.ToXmlString(false);
            File.WriteAllText("PubKey.xml", publicKey);

            var privatePublicKey = rsa.ToXmlString(true);
            File.WriteAllText("PrivPubKey.xml", privatePublicKey);

            //Encrypt data
            var originalData = "Dato Secreto!";
            byte[] data;
            using(MemoryStream ms = new MemoryStream()){
                using (StreamWriter sw = new StreamWriter(ms)){
                    sw.WriteLine(originalData);
                }
                data = ms.ToArray();
            }
            
            //RSA rsa2 = RSA.Create();
            byte[] encryptedData = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);

            // write to file encypted data
            using (FileStream fileStream = new("RSAFileEncrypted", FileMode.OpenOrCreate))
            {
                fileStream.Write(encryptedData, 0, encryptedData.Length);
            }

            Console.WriteLine("El archivo cifrado se generó con éxito!");

        }
        catch (CryptographicException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}