def main()

	puts "Enter your brackets. It might be [, ], (, ), {, } :"
	brackets = gets.chomp.split("")
	
	stack_of_elems = Array.new()

	result = true

	brackets.each do |element|
		if is_open_bracket(element) != nil
			stack_of_elems << element
		elsif close_bracket(stack_of_elems[-1]) == element
			stack_of_elems.pop
		else
			result = false
			break
		end
	end

	if stack_of_elems.length == 0 and result == true
		puts "True"
	else
		puts "False"
	end

end


def close_bracket(character)
	if character == "["
		return "]"
	elsif character == "("
		return ")"
	elsif character == "{"
		return "}"
	else 
		return nil
	end
end


def is_open_bracket(character)
	return character =~ /[\(\[\{]/
end


main()