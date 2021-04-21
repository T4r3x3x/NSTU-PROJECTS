      program main
                
        real min_x, max_x, step_x, min_y, max_y, step_y, i, j,x,y
        integer steps_x, steps_y
        logical IsSame 
        OPEN (1,FILE='C:\Users\hardb\Desktop\Input.txt')
        OPEN (2,FILE='C:\Users\hardb\Desktop\Output1.txt') 
        
        read (1,*) min_x 
        read (1,*) max_x
        read (1,*) step_x
        read (1,*) min_y
        read (1,*) max_y
        read (1,*) step_y
        
        x = -min_x-1
        y = -min_y-1
                
        steps_x = (max_x-min_x)/step_x
        steps_y = (max_y-min_y)/step_y       
        write(2,'(A$)') '             | '
        
        do i = 0, steps_x            
           if(IsSame(x,min_x + i*step_x)) then    
               x = min_x + i*step_x      
               write(2,'(e12.4$,(A$))') x, ' | '                  
           else   
               write(2,'(A$)') '             | ' 
           end if              
        end do
        
        if(x .LT. max_x) then
            write(2,'(e12.4$, (A$))') max_x, ' | '
        end if     
        
        write (2,*) ' '
        
        do i = 0, steps_y
            if(IsSame(y,min_y+i*step_y)) then 
               y = min_y + i*step_y
               write(2,'(e12.4$,(A$))') y, ' | '          
            
            x = -min_x-1
            do j = 0, steps_x
                if(IsSame(x,min_x + j*step_x)) then 
                    x = min_x + j*step_x                
                    call func(x,y)              
                else
                    write(2,'(A$)') '             | '
                end if
            end do 
            
            if(x .LT. max_x) then
                call Func(max_x,y)
            end if
            
            write (2,*) ' ' 
            
            else
            write(2,'(A$)') '             | '
            end if
                                   
        end do
        
        
        
        if(y .LT. max_y) then
                    write(2,'(e12.4$,(A$))') max_y, ' | '
                    y = max_y
                    x = -min_x-1
                    do j = 0, steps_x
                        if(IsSame(x,min_x + j*step_x)) then 
                            x = min_x + j*step_x                
                            call func(x,y)              
                        else 
                            write(2,'(A$)') '             | '                       
                        end if
                    end do 
            
                    if(x .LT. max_x) then
                        call Func(max_x,y)
                    end if
            end if  
        end 
        
        subroutine Func(x, y) 
            implicit none  
            real x,y,yt,xt,Pi
            parameter(pi = 3.1415926535)
            call Around(y,y) 
            if (y .EQ. 90.0) then
                write(2, '(A$)') '  div. by 0  | '      
            else
            call around(x*PI/180.0,xt)
            call around(y*PI/180.0,yt)
      write(2,'(e12.4$, A$)')abs(sin(xt)/cos(yt)),' | '                 
            end if             
            return
        end
        
               
                  
                 logical function IsSame(x,y)
                 implicit none
                 real x,y
                 call Around(x,x)
                 call Around(y,y)

                 if(x .EQ. y) then
                    IsSame = .FALSE.
                    return
                 else
                    IsSame = .TRUE.             
                    return
                  end if
                  end
                
                
                
                
                subroutine Around(x,result)
                implicit none
                real x, result 
                character*12 line
                write(line,'(e12.4)') x
                read(line,'(e12.4)')result           
                end
