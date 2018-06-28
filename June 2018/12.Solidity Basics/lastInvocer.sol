pragma solidity ^0.4.18;

contract LastInvocer {
    address private lastInvocer;
    
    function getLastInvocer() public returns (bool, address) {
        address result = lastInvocer;
        lastInvocer = msg.sender;
        return (result != 0x0, result);
    }
}