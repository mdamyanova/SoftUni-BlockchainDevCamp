pragma solidity ^0.4.18;

contract Owned {
    constructor() public { owner = msg.sender; }
    address internal owner;
}

contract Terminatable is Owned {
    function terminate() public {
        require (msg.sender == owner);
        selfdestruct(owner);
    }
}