pragma solidity ^0.4.18;

contract Certificates {
    mapping (string => bool) private certificateHashes;
    address contractOwner = msg.sender;
    
    function add(string hash) public {
        require (msg.sender == contractOwner);
        certificateHashes[hash] = true;
    }
    function verify(string hash) view public returns (bool) {
        return certificateHashes[hash];
    }
}