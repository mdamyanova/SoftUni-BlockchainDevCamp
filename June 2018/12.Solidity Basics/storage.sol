pragma solidity ^0.4.18;

contract SimpleStorage {
    uint private storedData;
    
    function set(uint x) public {
        storedData = x;
    }
    
    function get() constant public returns (uint) {
        return storedData;
    }
}