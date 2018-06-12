import org.web3j.crypto.Sign;
import org.web3j.crypto.Sign.SignatureData;

import java.math.BigInteger;
import java.security.SignatureException;

public class EthereumSignatureVerifier {

    public static void main(String[] args) throws SignatureException {

        // Input - address, signature, message
        String address = "0xa44f70834a711F0DF388ab016465f2eEb255dEd0";
        String signedMessage = "acd0acd4eabd1bec05393b33b4018fa38b69eba8f16ac3d60eec9f4d2abc127e3c92939e680b91b094242af80fce6f217a34197a69d35edaf616cb0c3da4265b01";
        String message = "exercise-cryptography";

        // TODO - get v, r, s from given signed message, find public key...

        // Recover public key
        //byte[] r = signedMessage.substring(0, 64).getBytes();
        //byte[] s = signedMessage.substring(64, signedMessage.length() - 2).getBytes();
        //byte v = (byte)signedMessage.charAt(signedMessage.length() - 1);

        //Sign.SignatureData signature = new SignatureData(v, r, s);

        //BigInteger pubKeyRecovered = Sign.signedMessageToKey(message.getBytes(), signature);
        //System.out.println("Recovered public key: " + pubKeyRecovered.toString(16));

        //boolean validSig = publicKey.equals(pubKeyRecovered);
        //System.out.println("Signature valid? " + validSig);
    }
}