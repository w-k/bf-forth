#! /usr/local/bin/gforth

43 Constant plus        \   +
45 Constant minus       \   -
60 Constant lt          \   <
62 Constant gt          \   >
44 Constant comma       \   ,
46 Constant period      \   .
91 Constant open_paren  \   [
93 Constant close_paren \   ]

create cellArray 30000 chars allot
cellArray 30000 chars erase
create stdinBuffer 1 chars allot
create srcArray 1000 chars allot

next-arg r/o open-file throw Value src
srcArray 1000 src read-file throw Value length

: readStdinChar ( stdinBuffer -- char )
  dup 1 stdin read-file throw drop
  c@
;

: getC ( index arrayAddr -- index arrayAddr val )
  over over swap 
  chars + c@
;

: updateC ( index arrayAddr val -- index )
  rot rot over
  chars + 
  rot swap
  c!
;

: findNext ( srcIndex query -- newSrcIndex ) 
  swap 1+
  begin
    srcArray getC 
    swap drop
    rot
    over over <> while
      rot 1+
      rot drop
  repeat
  drop drop
;

: findPrevious ( srcIndex query -- newSrcIndex ) 
  swap 1-
  begin
    srcArray getC 
    swap drop
    rot
    over over <> while
      rot 1-
      rot drop
  repeat
  drop drop
;

: handleChar ( cellIndex srcIndex character -- cellIndex srcIndex )

  dup plus = if
    rot
    cellArray getC 1+ updateC
    rot rot
  then

  dup minus = if
    rot
    cellArray getC 1- updateC
    rot rot
  then

  dup lt = if
    rot 
    1- 
    rot rot
  then

  dup gt = if
    rot
    1+ 
    rot rot
  then

  dup comma = if
    rot 
    cellArray stdinBuffer readStdinChar 
    updateC
    rot rot
  then

  dup period = if
    rot
    cellArray getC emit
    drop
    rot rot
  then

  dup open_paren = if
    rot 
    cellArray getC swap drop
    0 = if
      rot 
      close_paren findNext
      rot rot
    then
    rot rot
  then

  dup close_paren = if
    rot 
    cellArray getC swap drop
    0 <> if
      rot 
      open_paren findPrevious
      rot rot
    then
    rot rot
  then

  drop
  1+
;

: read ( -- )
  0 0 begin
    dup length < while 
      srcArray over chars + c@
      handleChar
  repeat
;

read 

bye
