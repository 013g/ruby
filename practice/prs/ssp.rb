class RockPaperScissorsMachine
  attr_reader :item

  ITEMS = %i[rock paper scissors]

  def self.initialize_machine_object(*args)
    new.initialize_machine_object
  end

  def self.initialize_user_object(*args)
    new.initialize_user_object(*args)
  end

  def initialize_machine_object
    @item = ITEMS.sample
    return self
  end

  def initialize_user_object(item)
    abort "invalid item #{item}" unless ITEMS.include?(item.to_sym)

    @item = item.to_sym
    return self
  end

  def <=>(other)
    return "victory"   if self_victory?(other)
    return "dead heat" if dead_heat?(other)
    return "defeat"    if self_defeat?(other)
    abort "error: self: #{self.item}, other: #{other.item}"
  end

  private
  def self_victory?(other)
    comparing_item_id = ITEMS.find_index(self.item) - 1
    ITEMS[comparing_item_id] == other.item
  end

  def dead_heat?(other)
    self.item == other.item
  end

  def self_defeat?(other)
    comparing_item_id = ITEMS.find_index(self.item) + 1

    # if element last in array (last+1 out from arr) should get first element
    comparing_item_id = 0 if comparing_item_id == ITEMS.size

    ITEMS[comparing_item_id] == other.item
  end
end

print "my item: "
user_item = STDIN.gets.chomp

machine = RockPaperScissorsMachine.initialize_machine_object
user =    RockPaperScissorsMachine.initialize_user_object(user_item)

print "Your choose is #{user.item} \n"
print "Machine choose is #{machine.item} \n"

p user <=> machine