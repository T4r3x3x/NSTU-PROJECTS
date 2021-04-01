        program main
        
        real min_x, max_x, step_x, min_y, max_y, step_y, i, j,x,y
        real temp
        integer steps_x, steps_y
        OPEN (1,FILE='C:\Users\pmi-b9205\Desktop\Input.txt')
        OPEN (2,FILE='C:\Users\pmi-b9205\Desktop\Output.txt') 
        
        read (1,*) min_x 
        read (1,*) max_x
        read (1,*) step_x
        read (1,*) min_y
        read (1,*) max_y
        read (1,*) step_y
                
        steps_x = (max_x-min_x)/step_x
        steps_y = (max_y-min_y)/step_y
        
        call IsInvisible(min_x,max_x)
        call Func(min_x,max_x)
        write(2,'(A$)') '             | '
        
         do i = 0, steps_x
            x = sum_rump(min_x,i*step_x)  
            write(2,'(e12.4$,(A$))') x, ' | '                   
        end do
        
        if(x .LT. max_x) then
            write(2,'(e12.4$, (A$))') max_x, ' | '
        end if     
        
        write (2,*) ' '
        
        do i = 0, steps_y
            y = sum_rump(min_y,i*step_y)
            write(2,'(e12.4$,(A$))') y, ' | '
            
            do j = 0, steps_x
                x = sum_rump(min_x,j*step_x)                
                call func(x,y)                
            end do 
            
            if(x .LT. max_x) then
                call Func(max_x,y)
            end if
            
            write (2,*) ' '
            
                
        end do
        if(y .LT. max_y) then
                    write(2,'(e12.4$,(A$))') max_y, ' | '
                    y = max_y
                    do j = 0, steps_x
                        x = sum_rump(min_x,j*step_x)                
                        call func(x,y)                        
                    end do 
            
                    if(x .LT. max_x) then
                        call Func(max_x,y)
                    end if
            end if            
        end    
        
        
        
        subroutine IsInvisible(x,y)
        implicit none
        real x,  y 
        character*10 number
        character*10 number_step
        write(number,'(F10.2)') x
        write(number_step,'(F10.2)') y
        write(2,'(A)')number
        end        
        
        
        subroutine Func(x, y)             
            real x,y 
            if(cos(y) .NE. 0) then
                write (2, '(e12.4$, A$)') abs(sin(x)/cos(y)),' | ' 
            else
                write(2, '(A$)') '  div. by 0  | '
            end if             
            return
        end
    

        real function sum_rump (x,y)
        implicit none
        real x,y,e,two_sum,c,s
        c = 0.0
        s = two_sum (e, x, y)
        c = c + e
        sum_rump = s + c
        end

        real function two_sum (t, a, b)
        implicit none
        real t,a,b,s,bs,as
         s = a + b
         bs = s - a
         as = s - bs
        t = (b - bs) + (a - as)
        two_sum = s
        end
