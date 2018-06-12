import org.bouncycastle.crypto.generators.SCrypt;
import org.bouncycastle.util.encoders.Hex;

public class KeyByPasswordSCrypt {

    public static void main(String[] args) {

        // Input - Password and Salt
        String password = "p@$$w0rd~3";
        String salt = "7b07a2977a473e84fc30d463a2333bcfea6cb3400b16bec4e17fe981c925ba4f";

        // Derive key from the password using SCrypt (16 384 iterations, block size 16, parallel factor 1)
        byte[] hash = SCrypt.generate(password.getBytes(), salt.getBytes(), 16384, 16, 1, 32);
        String pair = Hex.toHexString(hash);

        // Output - the pair (salt, derived key) from the hash algorithm
        System.out.println(pair);
    }
}