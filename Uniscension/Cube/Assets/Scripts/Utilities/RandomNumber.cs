using System;
using System.Security.Cryptography;

//Used to generate random numbers
public static class RandomNumber
{
	public static int Generate(int min, int max)
    {
        byte[] byteArray = new byte[4];
        //Using RNGCryptoServiceProvider to fill the byte array with random numbers
        (new RNGCryptoServiceProvider()).GetBytes(byteArray);
        //Applying the modulus operator to the random number so that it's within the specified min and max range, returned as an integer
        return (int)(BitConverter.ToUInt32(byteArray, 0) % max + min);
    }
}