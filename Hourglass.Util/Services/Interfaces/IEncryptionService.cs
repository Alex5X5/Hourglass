namespace Hourglass.Util.Services.Interfaces;

public interface IEncryptionService {

	public unsafe byte* EncryptBuffer(byte* inputData, int inputLength, out int outputBufferSize);

	public unsafe byte* DecryptBuffer(byte* inputData, int inputLength, out int outputBufferSize);

	public unsafe void EncryptFile(string path, string key);
	
	public unsafe void DecryptFile(string path, string key);

}
