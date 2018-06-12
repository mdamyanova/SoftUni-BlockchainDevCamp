import org.bouncycastle.util.encoders.Hex;
import org.web3j.crypto.*;
import java.math.BigInteger;

public class ECCExample {

    public static String compressPublicKey(BigInteger publicKey) {
        String publicKeyYPrefix = publicKey.testBit(0) ? "03" : "02";
        String publicKeyHex = publicKey.toString(16);
        String publicKeyX = publicKeyHex.substring(0, 64);

        return publicKeyYPrefix + publicKeyX;
    }

    public static void main(String[] args) throws Exception {
        //BigInteger privateKey = Keys.createEcKeyPair().getPrivateKey();
        BigInteger privateKey = new BigInteger("97ddae0f3a25b92268175400149d65d6887b9cefaf28ea2c078e05cdc15a3c0a", 16);
        BigInteger publicKey = Sign.publicKeyFromPrivate(privateKey);
        ECKeyPair keyPair = new ECKeyPair(privateKey, publicKey);

        System.out.println("Private key: " + privateKey.toString(16));
        System.out.println("Public key: " + publicKey.toString(16));
        System.out.println("Public key (compressed): " + compressPublicKey(publicKey));

        String message = "Message for signing";
        byte[] messageHash = Hash.sha3(message.getBytes());

        Sign.SignatureData signature = Sign.signMessage(messageHash, keyPair, false);

        System.out.println("Message: " + message);
        System.out.println("Message hash: " + Hex.toHexString(messageHash));
        System.out.printf("Signature: [v = %d, r = %s, s = %s]\n",
                signature.getV() - 27,
                Hex.toHexString(signature.getR()),
                Hex.toHexString(signature.getS()));

        System.out.println();

        BigInteger publicKeyRecovered = Sign.signedMessageToKey(message.getBytes(), signature);
        System.out.println("Recovered public key: " + publicKeyRecovered.toString(16));

        boolean validSignature = publicKey.equals(publicKeyRecovered);
        System.out.println("Signature is valid: " + validSignature);
    }
}