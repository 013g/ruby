def main()
	while true do
		puts "Введите первое число: "
		number = STDIN.gets.chomp
	
		begin 
			a = Integer(number)
		rescue ArgumentError
			puts "Невозможно привести к типу данных int"
		end

		puts "Введите второе число: "

		number = STDIN.gets.chomp

		begin 
			b = Integer(number)
		rescue ArgumentError
			puts "Невозможно привести к типу данных int"
		end


		puts "Выберите операцию +, -, /, *"
		choice = STDIN.gets.chomp
		begin
			if contain_elements(choice) == nil
				raise UknownChoiceError.new("Неизвестная операция")
			end
		end
		if choice == "+"
			puts "Сумма числа "+a.to_s+" и числа "+b.to_s+" равна " + sum_numbers(a,b).to_s
		elsif choice == "-"
			puts "Разность числа "+a.to_s+" и числа "+b.to_s+" равна " + diff_numbers(a,b).to_s
		elsif choice == "*"
			puts "Произведение числа "+a.to_s+" на число "+b.to_s+" равно " + mult_numbers(a,b).to_s
		else 
			puts "Частное от деления числа "+a.to_s+" на число "+b.to_s+" равно " + div_numbers(a,b).to_s
		end
	end
end 




def contain_elements(str_choice)
	str_choice =~ /[\/\+\-\*]/
end

def sum_numbers(a,b)
	return a+b
end 

def diff_numbers(a,b)
	return a-b
end

def mult_numbers(a,b)
	return a*b
end

def div_numbers(a,b)
	begin
		result = a/b
	rescue ZeroDivisionError => error
		result = 'Ошибка. Деление на 0'
	end
	return result
end

class UknownChoiceError < StandardError
	def initialize(msg="Неизвестная операция")
		super(msg)
	end
end


main()