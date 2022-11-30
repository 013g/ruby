file = File.open('./data.txt')

file_data = file.read

file_data = file_data.downcase.gsub("\n",' ')
file_data = file_data.split(/[\ \#\|\&\\]{1,1}/)


file_data.each do |i|
    i.slice!(/[\.\,\;]/)
end

words = {}

file_data.each { |i|
  if !words.key?(i)
    words[i] = 1
  else
    words[i] += 1
  end
}

puts words.sort_by {|k,v| v}.reverse.slice(0,3)

file.close
