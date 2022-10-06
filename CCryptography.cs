using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CCryptography
{
	public struct Keys
	{
		public string Key;

		public string IV;
	}

	public class Rijndael
	{
		private static byte[] savedKey;

		private static byte[] savedIV;

		public static byte[] Key
		{
			get
			{
				return savedKey;
			}
			set
			{
				savedKey = value;
			}
		}

		public static byte[] IV
		{
			get
			{
				return savedIV;
			}
			set
			{
				savedIV = value;
			}
		}

		private static void RdGenerateSecretKey(RijndaelManaged rdProvider)
		{
			if (savedKey == null)
			{
				rdProvider.KeySize = 256;
				rdProvider.GenerateKey();
				savedKey = rdProvider.Key;
			}
		}

		private static void RdGenerateSecretInitVector(RijndaelManaged rdProvider)
		{
			if (savedIV == null)
			{
				rdProvider.GenerateIV();
				savedIV = rdProvider.IV;
			}
		}

		public static string Encrypt(string originalStr, string SavedKeyString, string SavedIVString)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(originalStr);
			MemoryStream memoryStream = new MemoryStream(bytes.Length);
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			if (SavedKeyString == "")
			{
				RdGenerateSecretKey(rijndaelManaged);
			}
			else
			{
				savedKey = Convert.FromBase64String(SavedKeyString);
			}
			if (SavedIVString == "")
			{
				RdGenerateSecretInitVector(rijndaelManaged);
			}
			else
			{
				savedIV = Convert.FromBase64String(SavedIVString);
			}
			if (savedKey == null || savedIV == null)
			{
				throw new NullReferenceException("savedKey and savedIV must be non-null.");
			}
			ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor((byte[])savedKey.Clone(), (byte[])savedIV.Clone());
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] inArray = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			cryptoTransform.Dispose();
			rijndaelManaged.Clear();
			return Convert.ToBase64String(inArray);
		}

		public static string Decrypt(string encryptedStr, string SavedKeyString, string SavedIVString)
		{
			if (SavedKeyString.ToString() != "")
			{
				savedKey = Convert.FromBase64String(SavedKeyString);
				savedIV = Convert.FromBase64String(SavedIVString);
			}
			byte[] array = Convert.FromBase64String(encryptedStr);
			byte[] array2 = new byte[array.Length];
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			MemoryStream memoryStream = new MemoryStream(array);
			if (savedKey == null || savedIV == null)
			{
				throw new NullReferenceException("savedKey and savedIV must be non-null.");
			}
			ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor((byte[])savedKey.Clone(), (byte[])savedIV.Clone());
			CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
			cryptoStream.Read(array2, 0, array2.Length);
			memoryStream.Close();
			cryptoStream.Close();
			cryptoTransform.Dispose();
			rijndaelManaged.Clear();
			string @string = Encoding.ASCII.GetString(array2);
			return @string.Replace("\0", "");
		}
	}

	public static Keys GetKeys(string password, int keyBitLength)
	{
		Keys result = default(Keys);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		byte[] salt = new byte[10] { 1, 2, 23, 234, 37, 48, 134, 63, 248, 4 };
		using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 234))
		{
			if (!rijndaelManaged.ValidKeySize(keyBitLength))
			{
				throw new InvalidOperationException("Invalid size key");
			}
			result.Key = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(keyBitLength / 8));
			result.IV = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8));
		}
		return result;
	}
}
