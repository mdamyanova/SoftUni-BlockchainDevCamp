pragma solidity ^0.4.18;

contract SimpleBank {
    mapping (address => uint) private balances;
    address private owner = msg.sender;
    
    constructor() private {
        balances[owner] = msg.sender.balance;
    }
    event Deposit(
        address indexed _from,
        uint _value
    );
    function deposit() payable public returns(uint) {
        require(msg.value > 0); // Check amount to send
        require(balances[msg.sender] >= msg.value) // Check balance
        require(balance[address(this)] + msg.value >= balances[address(this)]); // Check overflow
        emit deposit(msg.sender, msg.value);
        balances[msg.sender] = msg.sender.balance;
        return balances[msg.sender];
    }
    function withdraw(uint amount) public returns(uint) {
        msg.sender.transfer(amount * 1 ether);
        balances[msg.sender] = msg.sender.balance;
        return balances[msg.sender];
    }
    function getBalance() public returns(uint) {
        return balances[msg.sender];
    }
}