import org.bouncycastle.crypto.digests.RIPEMD160Digest;
import org.bouncycastle.crypto.ec.ECPair;
import org.bouncycastle.jcajce.provider.digest.RIPEMD160;
import org.bouncycastle.util.encoders.Hex;
import org.web3j.crypto.ECKeyPair;
import org.web3j.crypto.Keys;
import org.web3j.crypto.Sign;

import java.math.BigInteger;
import java.security.InvalidAlgorithmParameterException;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;

public class PrivateKeyToAddress {

    public static void main(String[] args) throws Exception {

        // Random Private Key
        // BigInteger privateKey = Keys.createEcKeyPair().getPrivateKey();

        // Existing Private Key
        BigInteger privateKey = new BigInteger(
                "fe549dbcccfbd11e255f6037e1e640efaca0e19966ac77a592fdf06d295952a4",
                16);

        BigInteger publicKey = privateKeyToPublicKey(privateKey);
        String address = privateKeyToAddress(privateKey);
        System.out.println("Extracted public key: ");
        System.out.println(publicKey);
        System.out.println("Extracted blockchain address: ");
        System.out.println(address);
    }

    private static BigInteger privateKeyToPublicKey(BigInteger privateKey) {
        BigInteger publicKey = Sign.publicKeyFromPrivate(privateKey);
        return publicKey;
    }

    public static BigInteger compressPublicKey(BigInteger publicKey) {
        String publicKeyYPrefix = publicKey.testBit(0) ? "03" : "02";
        String publicKeyHex = publicKey.toString(16);
        String publicKeyX = publicKeyHex.substring(0, 64);
        return new BigInteger(publicKeyYPrefix + publicKeyX, 16);
    }

    private static String publicKeyToAddress(BigInteger publicKey) {
        byte[] bytes = publicKey.toByteArray();
        RIPEMD160Digest digest = new RIPEMD160Digest();
        digest.update(bytes, 0, bytes.length);
        byte[] result = new byte[digest.getDigestSize()];
        digest.doFinal(result, 0);
        return Hex.toHexString(bytes);
    }

    private static String privateKeyToAddress(BigInteger privateKey) {
        BigInteger publicKey = privateKeyToPublicKey(privateKey);
        BigInteger compressedPublicKey = compressPublicKey(publicKey);
        String address = publicKeyToAddress(compressedPublicKey);
        return address;
    }
}