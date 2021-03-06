        program main
        implicit none 
        real Pi
        parameter(pi = 3.1415926535)      
        real min_x, max_x, step_x, min_y, max_y, step_y, i, j,k
        integer steps_x, steps_y
        OPEN (1,FILE='C:\Users\hardb\Desktop\Input.txt')
        OPEN (2,FILE='C:\Users\hardb\Desktop\Output.txt') 
        
        read (1,*) min_x
        read (1,*) max_x
        read (1,*) step_x
        read (1,*) min_y
        read (1,*) max_y
        read (1,*) step_y
       
        if(max_x .LT. min_x) then
            write(2,*) 'x incorrent range'
            stop
        else if(max_y .LT. min_y) then
            write(2,*) 'y incorrent range'
            
        else if(step_x .EQ. 0) then
            write(2,*) 'step x incorrent range'
            stop
        else if(step_y .EQ. 0) then
            write(2,*) 'step x incorrent range' 
            stop
        else
            write(2,'(A$)') '             | '
            do k = min_x, max_x, step_x
                write(2,'(e12.4$, (A$))') k, ' | '
            end do 
            write(2,*) ' '
        
            do i = min_y, max_y, step_y
                write(2,'(e12.4$,(A$))') i, ' | '
                do j = min_x, max_x, step_x
                    call Func(j*pi/180.0,i*pi/180.0)     
                end do
                write (2,*) ' '
            end do
       end if 
       end
        
        
        subroutine Func(x, y)             
            real x,y 
            if(sin(y) .NE. 0) then
                write (2, '(e12.4$, A$)') abs(cos(x)/sin(y)),' | ' 
            else
                write(2, '(A$)') '  div. by 0  | '
            end if             
            return
        end
