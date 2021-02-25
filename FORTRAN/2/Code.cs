        program main
        implicit none 
        real Pi
        parameter(Pi = 3.1415926535)         
        real min_x, max_x, step_x, min_y, max_y, step_y, i, j
        integer steps_x, steps_y
        
        OPEN (1,FILE='C:\Users\pmi-b9205\Desktop\Output.txt') 
        
        print *, 'Enter min x'
        read *, min_x
        print *, 'Enter max x'
        read *, max_x
        print *, 'Enter step x'
        read *, step_x
        print *, 'Enter min y'
        read *, min_y
        print *, 'Enter max y'
        read *, max_y
        print *, 'Enter step y'
        read *, step_y
        
        steps_x = (max_x - min_x) / step_x
        steps_y = (max_y - min_y) / step_y
        write(1,'(A$)') '       | '
        do i = min_x, max_x, step_x
            write(1,'(f6.4$, (A$))') i, ' | '
        end do 
        write(1,*) ' '
        do i = min_y, max_y, step_y
            write(1,'(f6.4$,(A$))') i, ' | '
            do j = min_x, max_x, step_x
                call Func(j,i)     
            end do
            write (1,*) ' '
        end do
        close (1)
        end
        
        
        subroutine Func(x, y)   
            real x,y 
            if(sin(y) .NE. 0) then
                write (1, '(f6.4$, A$)') abs(cos(x)/sin(y)),' | ' 
            else
                write(1, '(A$)') 'div. 0 | '
            end if             
            return
        end
