pragma solidity ^0.4.18;

contract SimpleToken {
    mapping (address => uint256) public balanceOf;
    
    function simpleToken (uint256 initialSupply) public {
        balanceOf[msg.sender] = initialSupply;
    }
    function transfer(address to, uint256 value) public {
        require(balanceOf[msg.sender] >= value); // Check the sender balance
        require(balanceOf[to] + value >= balanceOf[to]); // Overflow chek
        balanceOf[msg.sender] -= value; // Subtract the value from the sender
        balanceOf[to] += value; // Add the same value to the recipient
    }
}