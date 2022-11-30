def main()
	puts "Enter your text: "
	user_string = STDIN.gets.chomp
	user_string = user_string.downcase

	from_alphabet_to_number(user_string)


end


def from_alphabet_to_number(user_string)
	result = ""
	for i in 0..user_string.length() do
		if is_a_letter(user_string[i]) != nil 
			result += (alphabet_position(user_string[i])-96).to_s+" "
		end
	end
	puts result
end



def alphabet_position(user_character)
	return user_character.ord
end


def is_a_letter(letter)
	letter =~ /[a-z]/
end


main()