namespace Client.Step01
{
	public static class Program
	{
		static Program()
		{
		}

		public static void Main()
		{
			// **************************************************
			//string passPhrase01 = "";
			//string plainText01 = "";

			//var result01 =
			//	EncryptString(passPhrase: passPhrase01, plainText: plainText01);

			//System.Console.WriteLine($"{nameof(result01)}: {result01}");
			// **************************************************

			// **************************************************
			//string passPhrase02 = "12345";
			//string plainText02 = "Hello, World!";

			//var result02 =
			//	EncryptString(passPhrase: passPhrase02, plainText: plainText02);

			//System.Console.WriteLine($"{nameof(result02)}: {result02}");
			// **************************************************

			// **************************************************
			//string passPhrase03 = "1234567812345678";
			//string plainText03 = "Hello, World!";

			//var result03 =
			//	EncryptString(passPhrase: passPhrase03, plainText: plainText03);

			//System.Console.WriteLine($"{nameof(result03)}: {result03}");

			//var result04 =
			//	DecryptString(passPhrase: passPhrase03, cypherText: result03!);

			//System.Console.WriteLine($"{nameof(result04)}: {result04}");
			// **************************************************

			// **************************************************
			string passPhrase04 = "1234567812345678";
			//string passPhrase04 = "1234567812345678mmm";
			//string passPhrase04 = "123456781234567812345678";
			//string passPhrase04 = "123456781234567812345678mmm";
			//string passPhrase04 = "12345678123456781234567812345678";
			//string passPhrase04 = "12345678123456781234567812345678mmm";

			string plainText04 = "Hello, World!";
			//string? initializationVector = System.Guid.NewGuid().ToString();
			//string? initializationVector = "encryptionIntVec";
			string? initializationVector = null;

			var result05 =
				EncryptString(passPhrase: passPhrase04,
				plainText: plainText04, initializationVector: initializationVector);

			System.Console.WriteLine($"{nameof(result05)}: {result05}");

			if (result05 != null)
			{
				var result06 =
					DecryptString(passPhrase: passPhrase04,
					cypherText: result05!, initializationVector: initializationVector);

				System.Console.WriteLine($"{nameof(result06)}: {result06}");
			}
			// **************************************************
		}

		/// <summary>
		/// AES algorithm supports 128, 198, and 256 bit encryption.
		/// </summary>
		public static string? EncryptString
			(string passPhrase, string plainText, string? initializationVector = null)
		{
			var plainTextByteArray =
				System.Text.Encoding.UTF8.GetBytes(s: plainText);

			var passPhraseByteArray =
				System.Text.Encoding.UTF8.GetBytes(s: passPhrase);

			byte[] cypherTextByteArray;

			// **************************************************
			byte[] iv;

			if (string.IsNullOrWhiteSpace(initializationVector))
			{
				iv =
					new byte[16];
			}
			else
			{
				iv =
					System.Text.Encoding.UTF8
					.GetBytes(s: initializationVector.Substring(startIndex: 0, length: 16));
			}
			// **************************************************

			using (var aes = System.Security.Cryptography.Aes.Create())
			{
				// **************************************************
				var validKeySize =
					aes.ValidKeySize(bitLength: passPhraseByteArray.Length * 8);

				if (validKeySize == false)
				{
					return null;
				}
				// **************************************************

				// **************************************************
				// **************************************************
				// **************************************************
				var keySize = aes.KeySize; // [256]
				var padding = aes.Padding; // [PKCS7]
				var blockSize = aes.BlockSize; // [128]
				var feedbackSize = aes.FeedbackSize; // [8]
				var mode = aes.Mode; // [CBC] - CFB - CTS - ECB
				var legalKeySizes = aes.LegalKeySizes; // [Skip Size: 64, Min Size: 128, Max Size: 256]
				var legalBlockSizes = aes.LegalBlockSizes; // [Skip Size: 0, Min Size: 128, Max Size: 128]

				// **************************************************
				//var ciphertextLengthCbc1 =
				//	aes.GetCiphertextLengthCbc
				//	(plaintextLength: plainText.Length,
				//	paddingMode: System.Security.Cryptography.PaddingMode.None);

				var ciphertextLengthCbc2 =
					aes.GetCiphertextLengthCbc
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.Zeros);

				var ciphertextLengthCbc3 =
					aes.GetCiphertextLengthCbc
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.PKCS7);

				var ciphertextLengthCbc4 =
					aes.GetCiphertextLengthCbc
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.ANSIX923);

				var ciphertextLengthCbc5 =
					aes.GetCiphertextLengthCbc
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.ISO10126);
				// **************************************************

				// **************************************************
				var ciphertextLengthCfb1 =
					aes.GetCiphertextLengthCfb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.None);

				var ciphertextLengthCfb2 =
					aes.GetCiphertextLengthCfb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.Zeros);

				var ciphertextLengthCfb3 =
					aes.GetCiphertextLengthCfb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.PKCS7);

				var ciphertextLengthCfb4 =
					aes.GetCiphertextLengthCfb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.ANSIX923);

				var ciphertextLengthCfb5 =
					aes.GetCiphertextLengthCfb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.ISO10126);
				// **************************************************

				// **************************************************
				//var ciphertextLengthEcb1 =
				//	aes.GetCiphertextLengthEcb
				//	(plaintextLength: plainText.Length,
				//	paddingMode: System.Security.Cryptography.PaddingMode.None);

				var ciphertextLengthEcb2 =
					aes.GetCiphertextLengthEcb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.Zeros);

				var ciphertextLengthEcb3 =
					aes.GetCiphertextLengthEcb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.PKCS7);

				var ciphertextLengthEcb4 =
					aes.GetCiphertextLengthEcb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.ANSIX923);

				var ciphertextLengthEcb5 =
					aes.GetCiphertextLengthCfb
					(plaintextLength: plainText.Length,
					paddingMode: System.Security.Cryptography.PaddingMode.ISO10126);
				// **************************************************
				// **************************************************
				// **************************************************

				aes.IV = iv; // IV is optional
				aes.Key = passPhraseByteArray;

				using (var encryptor = aes.CreateEncryptor())

				using (var memoryStream = new System.IO.MemoryStream())

				using (var cryptoStream =
					new System.Security.Cryptography.CryptoStream
					(stream: memoryStream, transform: encryptor,
					mode: System.Security.Cryptography.CryptoStreamMode.Write, leaveOpen: false))
				{
					cryptoStream.Write
						(buffer: plainTextByteArray,
						offset: 0, count: plainTextByteArray.Length);

					cryptoStream.FlushFinalBlock();

					cypherTextByteArray =
						memoryStream.ToArray();
				}
			}

			var cypherText =
				System.Convert.ToBase64String
				(inArray: cypherTextByteArray);

			return cypherText;
		}

		public static string? DecryptString
			(string passPhrase, string cypherText, string? initializationVector = null)
		{
			// **************************************************
			// دستور ذیل غلط است
			//var cypherTextByteArray =
			//	System.Text.Encoding.UTF8.GetBytes(s: cypherText);

			var cypherTextByteArray =
				System.Convert.FromBase64String(s: cypherText);
			// **************************************************

			var passPhraseByteArray =
				System.Text.Encoding.UTF8.GetBytes(s: passPhrase);

			byte[] plainTextByteArray;

			// **************************************************
			byte[] iv;

			if (string.IsNullOrWhiteSpace(initializationVector))
			{
				iv =
					new byte[16];
			}
			else
			{
				iv =
					System.Text.Encoding.UTF8
					.GetBytes(s: initializationVector.Substring(startIndex: 0, length: 16));
			}
			// **************************************************

			using (var aes = System.Security.Cryptography.Aes.Create())
			{
				// **************************************************
				var validKeySize =
					aes.ValidKeySize(bitLength: passPhraseByteArray.Length * 8);

				if (validKeySize == false)
				{
					return null;
				}
				// **************************************************

				aes.IV = iv; // IV is optional
				aes.Key = passPhraseByteArray;

				using (var decryptor = aes.CreateDecryptor())

				using (var memoryStream = new System.IO.MemoryStream())

				using (var cryptoStream =
					new System.Security.Cryptography.CryptoStream
					(stream: memoryStream, transform: decryptor,
					mode: System.Security.Cryptography.CryptoStreamMode.Write, leaveOpen: false))
				{
					cryptoStream.Write
						(buffer: cypherTextByteArray,
						offset: 0, count: cypherTextByteArray.Length);

					cryptoStream.FlushFinalBlock();

					plainTextByteArray =
						memoryStream.ToArray();
				}
			}

			// دستور ذیل غلط است
			//var plainText =
			//	System.Convert.ToBase64String
			//	(inArray: plainTextByteArray);

			string plainText =
				System.Text.Encoding.UTF8
				.GetString(bytes: plainTextByteArray);

			return plainText;
		}
	}
}
