import random
print('Rolling the dice.......')
print(random.randint(1, 9999))

again = input('Want to roll the dice again? : ')
while again == 'yes' or again == 'y':
     print('Rolling the dice.......')
     print(random.randint(1, 9999))
    break
else:
    break
    print('stop')
