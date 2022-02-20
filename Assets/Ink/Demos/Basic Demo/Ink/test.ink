Hello Welcome to our booth, what can I help you today? -> WelcomeChoices

== WelcomeChoices ==
+ [Who are you?] -> self_introduction
+ [What booth is this?] -> booth_introduction
+ [Nevermind] -> nevermind
    
== self_introduction ==
    I am Asep, here I will help you to know better about our new product -> WelcomeChoices
    
== booth_introduction ==
    This booth is about herb food that our company specially create. -> what_booth

== nevermind ==
    Thank you for visiting our booth. ->END

== what_booth ==    
++ [What kind of food is that?] -> detail_food
++ [Ahh, I see.] -> WelcomeChoices

== detail_food ==
It is just like your everyday food with the same taste, but using herb for the majority its inggredients -> WelcomeChoices


