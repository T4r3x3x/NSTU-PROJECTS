                program main
                implicit none
                real a,b,c
                common /triangle/a, b, c
                call menu
                end
                
                subroutine menu
                integer chose
                real square, min_angle, min_cos
                common /triangle/a, b, c
                print *, '1) Enter a new triangle'
                print *, '2) Calculate square'
                print *, '3) Calculate the minimal angle in degrees'
                print *, '4) Calculate the minimal cos'
                print *, '5) Exit'
                read*, chouse

                if(chouse.eq.1) then 
                    call enterTriangle
                else if(chouse.eq.2) then
                    print *, Square()
                else if(chouse.eq.3) then
                    print *, min_angle()
                else if(chouse.eq.4) then
                    print *, min_cos()
                else
                    stop              
                end if
                call menu
                return
                end
                
                subroutine enterTriangle
                common /triangle/a, b, c
                logical isExist
                print *, 'Enter the first side'
                read*, a
                print *, 'Enter the second side'
                read*, b
                print *, 'Enter the third side'
                read*, c
                if(isExist()) then
                    call menu
                else
                    print *, 'Triangle not exist'
                    call menu
                end if   
                return
                end
                
                
                function isExist()
                common /triangle/ a,b,c
                logical isExist
                isExist = .false.
                if(a .NE. 0 .AND. b .NE. 0 .AND. c .NE. 0) then
                    if(a + b .LE. c) then
                        return
                    else 
                        if(a + c .LE. b) then
                            return
                        else 
                            if(b + c .LE. a) then
                            return
                            else
                                isExist = .true.   
                            end if
                        end if               
                    end if
                else
                    if(a .EQ. 0 .AND. b .EQ.. 0 .AND. c .EQ. 0) then    
                        return
                    else
                        isExist = .true.   
                    end if
                end if
                return 
                end
                
                function Square()
                common /triangle/ a,b,c
                real Square, p 
                if(a .NE. 0 .AND. b .NE. 0 .AND. c .NE. 0) then
                    p = simePerimetr()
                    Square = sqrt(p*(p-a)*(p-b)*(p-c))
                else
                    Square = 0
                end if
                return 
                end
                
                function simePerimetr()
                common /triangle/ a,b,c
                real simePerimetr
                simePerimetr = (a+b+c)/2
                return 
                end 
                
                function min_angle()  
                common /triangle/ a,b,c     
                real min_angle
                if(a .NE. 0 .AND. b .NE. 0 .AND. c .NE. 0) then                                    
                    if(a .LE. b .AND. a .LE. c) then
                        min_angle = acos(get_cos(a,b,c))
                    else 
                        if(b .LE. c .AND. b .LE. c) then
                            min_angle = acos(get_cos(b,c,a)) 
                        else 
                            min_angle = acos(get_cos(c,a,b)) 
                        end if               
                    end if    
                else
                    min_angle = 0    
                end if    
                min_angle = min_angle*180/3.1416
                return                            
                end 
                
                function min_cos()
                common /triangle/ a,b,c     
                real min_cos
                min_cos =0
                if(a .NE. 0 .AND. b .NE. 0 .AND. c .NE. 0) then     
                    if(a .GE. b .AND. a .GE. c) then
                        min_cos = get_cos(a,b,c)
                    else 
                        if(b .GE. c .AND. b .GE. c) then
                            min_cos = get_cos(b,c,a)
                        else 
                            min_cos = get_cos(c,a,b)
                        end if
                    end if               
                else
                    min_cos = 1        
                end if    
                return 
                end 
                
                function get_cos(a,b,c)
                real a,b,c, get_cos
                get_cos = (b**2 + c**2 - a**2)/(2*b*c)
                return 
                end 
                
                

