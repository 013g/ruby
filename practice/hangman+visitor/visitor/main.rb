# Интерфейс компонента объявляет метод accept, который принимает в виде параметра любой объект реализующий интерфейс нашего посетителя
class Component
  # @abstract
  #
  # @param [Visitor] visitor
  def accept(_visitor)
    raise NotImplementedError, "#{self.class} has not implemented method '#{__method__}'"
  end
end

# Каждый конкретный компонент должен реализовывать метод accept таким образом, что бы он вызывал метод класса  соотвествующий компоненту
class ConcreteComponentA < Component
  #вызываем visitConcreteComponentA, что соответствует
  # названию класса таким образом мы позволяем узнать нашему посетителю с каким классом компонента он будет работать
  # @param [Visitor] visitor
  def accept(visitor)
    visitor.visit_concrete_component_a(self)
  end

  # конкретные компоненты могут иметь свои особые методы, но посетитель все равно может их использовать т.к знает о компоненте
  def exclusive_method_of_concrete_component_a
    'A'
  end
end


class ConcreteComponentB < Component
  # @param [Visitor] visitor
  def accept(visitor)
    visitor.visit_concrete_component_b(self)
  end

  def special_method_of_concrete_component_b
    'B'
  end
end

# класс посетитель содержит методы соответствующее каждому из компонентов
# @abstract
class Visitor
  # @abstract
  #
  # @param [ConcreteComponentA] element
  def visit_concrete_component_a(_element)
    raise NotImplementedError, "#{self.class} has not implemented method '#{__method__}'"
  end

  # @abstract
  #
  # @param [ConcreteComponentB] element
  def visit_concrete_component_b(_element)
    raise NotImplementedError, "#{self.class} has not implemented method '#{__method__}'"
  end
end

# конкретные версии посетителя содержат отдельные версии одного и того же алгоритма соответствующего каждому из компонентов
class ConcreteVisitor1 < Visitor
  def visit_concrete_component_a(element)
    puts "#{element.exclusive_method_of_concrete_component_a} + #{self.class}"
  end

  def visit_concrete_component_b(element)
    puts "#{element.special_method_of_concrete_component_b} + #{self.class}"
  end
end

class ConcreteVisitor2 < Visitor
  def visit_concrete_component_a(element)
    puts "#{element.exclusive_method_of_concrete_component_a} + #{self.class}"
  end

  def visit_concrete_component_b(element)
    puts "#{element.special_method_of_concrete_component_b} + #{self.class}"
  end
end

# Клиентский код может выполнять действия над набором элементов не выясняя их конкретных классов
# @param [Array<Component>] components
# @param [Visitor] visitor
def client_code(components, visitor)
  # ...
  components.each do |component|
    component.accept(visitor)
  end
  # ...
end

components = [ConcreteComponentA.new, ConcreteComponentB.new]

puts 'The client code works with all visitors via the base Visitor interface:'
visitor1 = ConcreteVisitor1.new
client_code(components, visitor1)

puts 'It allows the same client code to work with different types of visitors:'
visitor2 = ConcreteVisitor2.new
client_code(components, visitor2)