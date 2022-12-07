
def main()
  win = false
  word = get_word()
  user_string = "*"*word.length()
  lives_amount = 6
  while lives_amount != 0 do
    puts "The word is "+user_string
    puts "You have lives: "+lives_amount.to_s
    puts "Enter supposed letter: "
    letter = STDIN.gets.chomp
    if check_letter(letter,word,user_string) == false
      lives_amount -= 1
    end
    if has_unknown_letters(user_string) == nil
      win = true
      break
    end
  end

  if win == true
    puts "Congratulations! You win! The world was "+word
  else
    puts "Sorry! You've lost! The world was "+word
  end

end


def get_word()
  words = ['computer','network','algorithm','abstraction','intelligence','class','collection','dictionary','computation']

  return words[rand(words.length())]
end

def check_letter(letter,word,user_string)
  result = false
  for i in 0..word.length()
    if letter == word[i]
      user_string[i] = letter
      result = true
    end
  end
  return result
end

def has_unknown_letters(user_string)
  user_string =~ /[\*]/
end


main()