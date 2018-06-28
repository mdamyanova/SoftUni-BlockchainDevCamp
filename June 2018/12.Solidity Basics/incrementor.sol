pragma solidity ^0.4.18;

contract Incrementor {
    uint private value;
    
    function get() view public returns (uint) {
        return value;
    }
    
    function increment(uint delta) public {
        value += delta;
    }
}