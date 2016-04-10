var $jst = jQuery.noConflict();
$jst(function () {
   if ($jst("#AppearedGreNo").is(":checked")) $jst("#gre_score").hide();
    $jst("#AppearedGreNo").click(function () {
        $jst("#gre_score").hide();
        $jst(".gre").prop('required', false); $jst('.gre').val('');
        $jst("#greAwa").prop('required', false); $jst('#greAwa').val('');
    });
    $jst("#AppearedGre").click(function () {
        $jst("#gre_score").show();
        $jst(".gre").prop('required', true); $jst(".gre").attr({ type: 'number', min: '130', max: '170' });
        $jst("#greAwa").prop('required', true); $jst("#greAwa").attr({ type: 'number', min: '0', max: '6' });
    });
    if ($jst("#none").is(":checked")) $jst("#en_exam_score").hide();
    $jst("#none").click(function () {
        $jst("#en_exam_score").hide();
        $jst("tl").prop('required', false); $jst('.tl').val('');
    });
    $jst("#toefl").click(function () {
        $jst("#en_exam_score").show();
        $jst(".tl").prop('required', true); $jst(".tl").attr({type:'number',min:'0',max:'30'});
    });
    $jst("#ielts").click(function () {
       $jst("#en_exam_score").show();
       $jst(".tl").prop('required', true); $jst(".tl").attr({ type: 'number', min: '0', max: '8' });
    });
    $jst("#en-exam-key-1").click(function () {
        $jst("#en-exam-score").val(""); $jst("#en-exam-score").attr({ type: 'number', min: '1', max: '120' });
    });
    $jst("#en-exam-key-2").click(function () {
        $jst("#en-exam-score").val(""); $jst("#en-exam-score").attr({ type: 'number', min: '1', max: '9' });
    });
});