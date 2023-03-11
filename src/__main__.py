import math
import random


SOURCE = [1,2,3,4,5,6,7,8,9]

# MAIN_SET = set(SOURCE)


def main():
    ng = NumberGenerator(target=12, source=SOURCE)
    # print(ng.__run_test(3, 12, main_set))
    print(ng.run())
    # print(calculate_Mulitples(24))


def calculate_Mulitples(inp:int)->list[tuple]:
    target = inp;

    output = set()

    for i in range(1, target+1):
        if target % i == 0:
            output.add((i, int(target/i)))
    return output



class NumberGenerator:
    def __init__(self, target=24, source:set=set())->None:
        self.target = target;
        self.multiples = self.__calculate_Mulitples()
        self.r_multiples = [(24,1), (12, 2), (8,3), (6,4)]
        self.main_set = source
        self.running_set = None

    def __remove_entry(self, n:int, set_n:set)->set:
        # make a copy of the set, to not mess with the original
        new_set = set_n.copy()
        # remove the entry from the copied set, and return the set
        try:
            new_set.remove(n)
            return new_set
        except KeyError as ke:
            return set_n
        
    
    def __calculate_Mulitples(self)->list[tuple]:
        output = []
        for i in range(1, self.target+1):
            if self.target % i == 0:
                output.append((i, int(self.target/i)))
        return output
        
    def __run_test(self, in_v:int, target:int, test_set:set)->dict:
        j = 0
        output_list = set()
        # test the first condition |x - t| = j
        j = abs(in_v - target)
        if j in test_set:
            output_list.add(int(j))
        
        j = in_v/target
        if j in test_set:
            output_list.add(int(j))
        
        j = target/in_v
        if j in test_set:
            output_list.add(int(j))
        
        j = target * in_v
        if j in test_set:
            output_list.add(int(j))

        return output_list
    
    def __generate_combos(self, input_pair:tuple, target_pair:tuple, test_set:set)->list:
      
        try:
            test_set.remove(input_pair[0])
            test_set.remove(input_pair[1])
        except Exception as e:
            pass

        fixed_set = test_set
        output_list = []
        # fixed set is the set without x, and y in it.
        # first generate the list of j's in the (x, y, j, i) - > combination
        # but the j's can only be valid when they have corresponding i's
        j_input = input_pair[0]
        j_target = target_pair[0]
        j_list = self.__run_test(j_input, j_target, fixed_set)

        # Get the I's for the corresponding J
        # Remember: J only affects the set, not the computation.
        i_input = input_pair[1]
        i_target = target_pair[1]

        for j in j_list:
            temp_set = self.__remove_entry(j, fixed_set)
            i_jth_list = self.__run_test(i_input, i_target, temp_set)

            if len(i_jth_list) > 0: # add it to the output only if it has a corresponding i
                res = (j, i_jth_list)
                output_list.append(res)
        return output_list

    def __generate_all_combinations(self, input_pair:tuple)->list:
        
        output = []
        for target_pair in self.multiples:
            # the running set is the complete set without the x, y inputs that were chosen at random.
            res = self.__generate_combos(input_pair, target_pair, self.running_set)
            if len(res) > 0:
                output.append([(input_pair[0], input_pair[1]), res])
        
        # run the algorithm in reverse.
        # for target_pair in self.r_multiples:
        #     # the running set is the complete set without the x, y inputs that were chosen at random.
        #     res = self.__generate_combos(input_pair, target_pair, self.running_set)
        #     if len(res) > 0:
        #         output.append([(input_pair[0], input_pair[1]), res])

        return output
    
    def run(self):
       # generate x, and y randomly from the set,
       # initialize the running set
       # start the generator
       # create output
       x = random.choice(self.main_set)
       self.main_set.remove(x)
       y = random.choice(self.main_set)
       self.main_set.remove(y)
       self.running_set = self.main_set.copy()

       output_space  = self.__generate_all_combinations((x,y)) 

       out_set = random.choice(output_space)

       output_ready = []
       rand = out_set[0]
       output_ready.append(rand[0])
       output_ready.append(rand[1])

       j_i = out_set[1]
       j_i_r = random.choice(j_i)
       output_ready.append(j_i_r[0])
       i_th = j_i_r[1]
       output_ready.append(random.choice(list(i_th)))

       return output_ready

            
if __name__ == '__main__':
    main()

