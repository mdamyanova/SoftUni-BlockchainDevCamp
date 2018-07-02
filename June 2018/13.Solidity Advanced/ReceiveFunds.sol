pragma solidity ^0.4.18;

contract ReceiveFunds {
    address private owner;
    
    // Set onwer in constructor
    constructor() public {
        owner = msg.sender;
    }
    modifier onlyOwner { 
        assert(msg.sender == owner);
        _; // Continue executing rest of method body
    } 
    event Deposit(
        address indexed _from,
        uint _value
    );
    // Fallback function
    function () public payable {
        assert(msg.value>0);
        emit Deposit(msg.sender,msg.value);
    }
    function send() payable public onlyOwner{
        msg.sender.transfer(address(this).balance);
        
    }
    function getBalance() view public returns(uint){
        return address(this).balance;
    }
}