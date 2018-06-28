pragma solidity ^0.4.18;

contract Facts {
    string[] private facts;
    address private contactOwner = msg.sender;
    
    function add(string newFact) public {
        require (msg.sender == contactOwner)
        facts.push(newFact);
    }
    function count() view public returns (uint) {
        return facts.length;
    }
    function getFact(uint index) view public returns (string) {
        return facts[index];
    }
}