



class A:
    
    x = 1

    def __init__(self):
        print("A.__init__")
    
    def __del__(self):
        print("A.__del__")



print(A.x)

