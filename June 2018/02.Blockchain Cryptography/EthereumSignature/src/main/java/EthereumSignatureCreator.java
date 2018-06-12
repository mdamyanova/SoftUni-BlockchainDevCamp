import java.math.BigInteger;
import org.bouncycastle.util.encoders.Hex;
import org.web3j.crypto.*;

public class EthereumSignatureCreator {

    public static void main(String[] args) throws Exception {
        // Input - private key and text message
        String privateKeyInput = "97ddae0f3a25b92268175400149d65d6887b9cefaf28ea2c078e05cdc15a3c0a";
        String message = "exercise-cryptography";

        BigInteger privateKey = new BigInteger(privateKeyInput, 16);

        // Use the web3j library to create public key from the private key
        BigInteger publicKey = Sign.publicKeyFromPrivate(privateKey);

        // Generate key pair with Eliptic Curve SECP-256k1
        ECKeyPair keyPair = new ECKeyPair(privateKey, publicKey);

        // Sign message using web3j built-in methods
        byte[] messageHash = Hash.sha3(message.getBytes());
        Sign.SignatureData signature = Sign.signMessage(messageHash, keyPair, false);

        // Get the values of transaction's signature - v (recovery id), r and s (outputs of the signature)
        int v = signature.getV() - 27;

        // Get the byte arrays and convert it to hexadecimal string
        String r = Hex.toHexString(signature.getR());
        String s = Hex.toHexString(signature.getS());

        // Output - JSON with signed message + signature[v, r, s]
        //JSONObject result = new JSONObject();
        //result.put("v", v);
        //result.put("r", r);
        //result.put("s", s);

        // more readable output
        System.out.printf("Signature: [v = %d, r = %s, s = %s]\n", v, r, s);
    }
}